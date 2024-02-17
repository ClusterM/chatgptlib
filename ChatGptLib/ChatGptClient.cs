using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using wtf.cluster.ChatGptLib.Types;
using wtf.cluster.ChatGptLib.Types.Content;
using wtf.cluster.ChatGptLib.Types.JsonSchema;
using wtf.cluster.ChatGptLib.Types.Tools;

namespace wtf.cluster.ChatGptLib
{
    /// <summary>
    /// The client for the GPT API
    /// </summary>
    public class ChatGptClient
    {
        const string Endpoint = "https://api.openai.com/v1/chat/completions";

        private readonly HttpClient client;

        /// <summary>
        /// Main constructor
        /// </summary>
        /// <param name="apiKey">OpenAI API key</param>
        public ChatGptClient(string apiKey)
        {
            client = new();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
        }

        /// <summary>
        /// The request to the OpenAI API (non-streaming)
        /// </summary>
        /// <param name="request">The ChatRequest object describing request data</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The response from the OpenAI API as ChatResponse object</returns>
        public async Task<ChatResponse> RequestAsync(ChatRequest request, CancellationToken cancellationToken = default)
        {
            var options = GetJsonOptions();
            var jsonString = JsonSerializer.Serialize(request, options);
            jsonString = jsonString.Insert(1, "\"stream\":false, "); // oh, crutch
            var contentString = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, Endpoint);
            requestMessage.Content = contentString;
            using var response = await client.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);
            var responseString = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            // Error check
            try
            {
                var errorContainer = JsonSerializer.Deserialize<ChatGptErrorContainer>(responseString);
                if (errorContainer?.Error != null)
                    throw new ChatGptException(errorContainer.Error);
            }
            catch (JsonException)
            {
                throw new InvalidDataException($"Can't parse JSON: {responseString}");
            }
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(responseString, inner: null, statusCode: response.StatusCode);
            // Deserialize!
            var result = JsonSerializer.Deserialize<ChatResponse>(responseString, options);
            if (result == null)
                throw new JsonException($"Can't parse JSON: {result}");
            return result;
        }

        /// <summary>
        /// The request to the OpenAI API (non-streaming)
        /// </summary>
        /// <param name="request">The ChatRequest object describing request data</param>
        /// <returns>The response from the OpenAI API as ChatResponse object</returns>
        public ChatResponse Request(ChatRequest request)
            => RequestAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

        /// <summary>
        /// The steaming request to the OpenAI API.
        /// </summary>
        /// <param name="request">The ChatRequest object describing request data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The response from the OpenAI API as a ChatResponse enumerator with partial data chunks.</returns>
        public async IAsyncEnumerable<ChatResponse> RequestStreamAsync(ChatRequest request, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var options = GetJsonOptions();
            var jsonString = JsonSerializer.Serialize(request, GetJsonOptions());
            jsonString = jsonString.Insert(1, "\"stream\":true, "); // oh, crutch
            var contentString = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, Endpoint);
            requestMessage.Content = contentString;
            using var response = await client.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
            using var stream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
            using StreamReader reader = new StreamReader(stream);
            string? line;
            while ((line = await reader.ReadLineAsync(cancellationToken).ConfigureAwait(false)) != null)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;
                if (line.StartsWith("data: "))
                    line = line[6..].Trim();
                else // in case of error we need to read it to the end
                    line += await reader.ReadToEndAsync(cancellationToken).ConfigureAwait(false);
                if (line == "[DONE]")
                    break;
                // Error check
                try
                {
                    var errorContainer = JsonSerializer.Deserialize<ChatGptErrorContainer>(line);
                    if (errorContainer?.Error != null)
                        throw new ChatGptException(errorContainer.Error);
                }
                catch (JsonException)
                {
                    throw new InvalidDataException($"Can't parse JSON: {line}");
                }
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(line, inner: null, statusCode: response.StatusCode);
                // Deserialize!
                var result = JsonSerializer.Deserialize<ChatResponse>(line, options);
                if (result == null)
                    throw new JsonException($"Can't parse JSON: {result}");
                yield return result;
            }
            yield break;
        }

        /// <summary>
        /// Create and return a JsonSerializerOptions object for serializing and deserializing all ChatGptLib structures.
        /// </summary>
        /// <returns>JsonSerializerOptions object.</returns>
        public static JsonSerializerOptions GetJsonOptions()
        {
            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                WriteIndented = true,
            };
            options.Converters.Add(new IJsonSchema.JsonSchemaConverter());
            options.Converters.Add(new IChatContent.ChatContentConverter());
            options.Converters.Add(new IChatContentPart.ChatContentPartConverter());
            options.Converters.Add(new IChatTool.ChatToolConverter());
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            return options;
        }

    }
}
