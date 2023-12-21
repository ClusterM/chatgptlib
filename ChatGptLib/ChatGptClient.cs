using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using wtf.cluster.ChatGptLib.Types;
using wtf.cluster.ChatGptLib.Types.Content;
using wtf.cluster.ChatGptLib.Types.JsonSchema;

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
        /// <returns>The response from the OpenAI API as ChatResponse object</returns>
        /// <exception cref="ChatGptException"></exception>
        /// <exception cref="HttpRequestException"></exception>
        /// <exception cref="InvalidDataException"></exception>
        public async Task<ChatResponse> RequestAsync(ChatRequest request)
        {
            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            options.Converters.Add(new IJsonSchema.JsonSchemaConverter());
            options.Converters.Add(new IChatContent.ChatContentConverter());
            options.Converters.Add(new IChatContentPart.ChatContentPartConverter());
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            var jsonString = JsonSerializer.Serialize(request, options);
            jsonString = jsonString.Insert(1, "\"stream\":false, "); // oh, crutch
            var contentString = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, Endpoint);
            requestMessage.Content = contentString;
            using var response = await client.SendAsync(requestMessage);
            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            // Error check
            var errorContainer = JsonSerializer.Deserialize<ChatGptErrorContainer>(responseString);
            if (errorContainer?.Error != null)
                throw new ChatGptException(errorContainer.Error);
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(responseString, inner: null, statusCode: response.StatusCode);
            // Deserialize!
            var result = JsonSerializer.Deserialize<ChatResponse>(responseString, options);
            if (result == null)
                throw new InvalidDataException($"Can't parse JSON: {result}");
            return result;
        }

        /// <summary>
        /// The steaming request to the OpenAI API
        /// </summary>
        /// <param name="request">The ChatRequest object describing request data</param>
        /// <returns>The response from the OpenAI API as a ChatResponse enumerator with partial data chunks</returns>
        /// <exception cref="ChatGptException"></exception>
        /// <exception cref="HttpRequestException"></exception>
        /// <exception cref="InvalidDataException"></exception>
        public async IAsyncEnumerable<ChatResponse> RequestStreamAsync(ChatRequest request)
        {
            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            options.Converters.Add(new IJsonSchema.JsonSchemaConverter());
            options.Converters.Add(new IChatContent.ChatContentConverter());
            options.Converters.Add(new IChatContentPart.ChatContentPartConverter());
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            var jsonString = JsonSerializer.Serialize(request, options);
            jsonString = jsonString.Insert(1, "\"stream\":true, "); // oh, crutch
            var contentString = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, Endpoint);
            requestMessage.Content = contentString;
            using var response = await client.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead);
            using var stream = await response.Content.ReadAsStreamAsync();
            using StreamReader reader = new StreamReader(stream);
            string? line;
            while ((line = await reader.ReadLineAsync().ConfigureAwait(false)) != null)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;
                if (line.StartsWith("data: "))
                    line = line[6..].Trim();
                else // in case of error we need to read it to the end
                    line += await reader.ReadToEndAsync();
                if (line == "[DONE]")
                    break;
                // Error check
                var errorContainer = JsonSerializer.Deserialize<ChatGptErrorContainer>(line);
                if (errorContainer?.Error != null)
                    throw new ChatGptException(errorContainer.Error);
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(line, inner: null, statusCode: response.StatusCode);
                // Deserialize!
                var result = JsonSerializer.Deserialize<ChatResponse>(line, options);
                if (result == null)
                    throw new InvalidDataException($"Can't parse JSON: {result}");
                yield return result;
            }
            yield break;
        }
    }
}
