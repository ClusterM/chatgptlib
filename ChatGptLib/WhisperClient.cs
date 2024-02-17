using System.Net.Http.Headers;
using System.Text.Json;
using wtf.cluster.ChatGptLib.Types;

namespace wtf.cluster.ChatGptLib
{
    /// <summary>
    /// Represents a class for interacting with the OpenAI Whisper API for audio transcription.
    /// </summary>
    public class WhisperClient
    {
        const string Endpoint = "https://api.openai.com/v1/audio/transcriptions";

        /// <summary>
        /// ID of the model to use. Only whisper-1 (which is powered by our open source Whisper V2 model) is currently available.
        /// </summary>
        public string Model { get; set; } = "whisper-1";

        /// <summary>
        /// The language of the input audio. Supplying the input language in ISO-639-1 format will improve accuracy and latency.
        /// </summary>
        public string? Language { get; set; } = null;

        /// <summary>
        /// The sampling temperature, between 0 and 1. Higher values like 0.8 will make the output more random, while lower values like 0.2 will make it more focused and deterministic. If set to 0, the model will use log probability to automatically increase the temperature until certain thresholds are hit.
        /// </summary>
        public double? Temperature { get; set; } = null;

        private readonly HttpClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="WhisperClient"/> class.
        /// </summary>
        /// <param name="apiKey">The OpenAI API key.</param>
        public WhisperClient(string apiKey)
        {
            client = new();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        }

        /// <summary>
        /// Change API key.
        /// </summary>
        /// <param name="apiKey">OpenAI API key.</param>
        public void SetApiKey(string apiKey) => client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        /// <summary>
        /// Transcribes audio data asynchronously.
        /// </summary>
        /// <param name="audioData">The raw audio data to be transcribed.</param>
        /// <param name="filename">The filename associated with the audio data, used for form submission.</param>
        /// <param name="cancellationToken">An optional token to observe while waiting for the task to complete.</param>
        /// <returns>A string representing the transcribed text from the audio data.</returns>
        /// <exception cref="ChatGptException">Thrown when the server returns an error response specific to the GPT model.</exception>
        /// <exception cref="InvalidDataException">Thrown when the response cannot be parsed as JSON.</exception>
        /// <exception cref="HttpRequestException">Thrown when the response indicates a failure with the HTTP request.</exception>
        /// <exception cref="JsonException">Thrown when the response JSON does not contain the expected "text" field.</exception>
        public async Task<string> AudioTranscriptionAsync(byte[] audioData, string filename, CancellationToken cancellationToken = default)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, Endpoint);
            var multipartContent = new MultipartFormDataContent
            {
                { new StringContent(Model), "model" }
            };
            if (Language != null)
                multipartContent.Add(new StringContent(Language), "language");
            if (Temperature != null)
                multipartContent.Add(new StringContent($"{Temperature}"), "temperature");
            multipartContent.Add(new ByteArrayContent(audioData), "file", filename);
            request.Content = multipartContent;
            var response = await client.SendAsync(request, cancellationToken);
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
            var result = JsonDocument.Parse(responseString).RootElement;
            if (!result.TryGetProperty("text", out var text))
                throw new JsonException($"JSON has no \"text\" field: {responseString}");
            return $"{text.GetString()}";
        }
    }
}
