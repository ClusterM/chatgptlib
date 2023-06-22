using System.Text.Json.Serialization;

namespace wtf.cluster.ChatGptLib.Types
{
    /// <summary>
    /// The usage statistics of how many tokens have been used for this request.
    /// </summary>
    public class ChatUsage
    {
        /// <summary>
        /// How many tokens did the prompt consist of.
        /// </summary>
        [JsonPropertyName("prompt_tokens")]
        public int PromptTokens { get; init; }

        /// <summary>
        /// How many tokens did the result consist of.
        /// </summary>
        [JsonPropertyName("completion_tokens")]
        public int CompletionTokens { get; init; }

        /// <summary>
        /// How many tokens did the request consume total.
        /// </summary>
        [JsonPropertyName("total_tokens")]
        public int TotalTokens { get; init; }

        /// <summary>
        /// The constructor for internal usage.
        /// </summary>
        /// <param name="promptTokens">How many tokens did the prompt consist of.</param>
        /// <param name="completionTokens">How many tokens did the result consist of.</param>
        /// <param name="totalTokens">How many tokens did the request consume total.</param>
        [JsonConstructor]
        public ChatUsage(int promptTokens, int completionTokens, int totalTokens)
        {
            PromptTokens = promptTokens;
            CompletionTokens = completionTokens;
            TotalTokens = totalTokens;
        }
    }
}
