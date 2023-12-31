using System.Text.Json.Serialization;
using wtf.cluster.ChatGptLib.Types.Tools;

namespace wtf.cluster.ChatGptLib.Types
{
    /// <summary>
    /// The request for the API, includes model name, messages and some options.
    /// </summary>
    public class ChatRequest
    {
        /// <summary>
        /// ID of the model to use. See the model endpoint compatibility table for details on which models work with the Chat API.
        /// </summary>
        [JsonPropertyName("model")]
        public string? Model { get; set; }

        /// <summary>
        /// A list of messages comprising the conversation so far.
        /// </summary>
        [JsonPropertyName("messages")]
        public IList<ChatMessage> Messages { get; set; } = new List<ChatMessage>();

        /// <summary>
        /// A list of functions the model may generate JSON inputs for.
        /// </summary>
        [JsonPropertyName("functions")]
        [Obsolete("Use 'tools'")]
        public IList<ChatFunction>? Functions { get; set; }

        /// <summary>
        /// A list of tools the model may call. Currently, only functions are supported as a tool. Use this to provide a list of functions the model may generate JSON inputs for.
        /// </summary>
        [JsonPropertyName("tools")]
        public IList<IChatTool>? Tools { get; set; }

        /// <summary>
        /// What sampling temperature to use, between 0 and 2. Higher values like 0.8 will make the output more random, while lower values like 0.2 will make it more focused and deterministic.
        /// Default is 1.
        /// </summary>
        [JsonPropertyName("temperature")]
        public double? Temperature { get; set; }

        /// <summary>
        /// An alternative to sampling with temperature, called nucleus sampling, where the model considers the results of the tokens with top_p probability mass. So 0.1 means only the tokens comprising the top 10% probability mass are considered.
        /// Default is 0.5.
        /// </summary>
        [JsonPropertyName("top_p")]
        public double? TopP { get; set; }

        /// <summary>
        /// How many chat completion choices to generate for each input message.
        /// </summary>
        [JsonPropertyName("n")]
        public int? N { get; set; }

        /*
        /// <summary>
        /// If set, partial message deltas will be sent, like in ChatGPT. Tokens will be sent as data-only server-sent events as they become available, with the stream terminated by a data: [DONE] message. 
        /// </summary>
        [JsonPropertyName("stream")]
        public bool? Stream { get; internal set; }
        */

        /// <summary>
        /// Up to 4 sequences where the API will stop generating further tokens.
        /// </summary>
        [JsonPropertyName("stop")]
        public IList<string>? Stop { get; set; }

        /// <summary>
        /// The maximum number of tokens to generate in the chat completion.
        /// </summary>
        [JsonPropertyName("max_tokens")]
        public int? MaxTokens { get; set; }

        /// <summary>
        /// Number between -2.0 and 2.0. Positive values penalize new tokens based on whether they appear in the text so far, increasing the model's likelihood to talk about new topics.
        /// Default is 0.
        /// </summary>
        [JsonPropertyName("presence_penalty")]
        public double? PresencePenalty { get; set; }

        /// <summary>
        /// Number between -2.0 and 2.0. Positive values penalize new tokens based on their existing frequency in the text so far, decreasing the model's likelihood to repeat the same line verbatim.
        /// Default is 0.
        /// </summary>
        [JsonPropertyName("frequency_penalty")]
        public double? FrequencyPenalty { get; set; }

        /// <summary>
        /// Modify the likelihood of specified tokens appearing in the completion. Accepts a json object that maps tokens(specified by their token ID in the tokenizer) to an associated bias value from -100 to 100. 
        /// Mathematically, the bias is added to the logits generated by the model prior to sampling.The exact effect will vary per model, but values between -1 and 1 should decrease or increase likelihood of selection.
        /// Values like -100 or 100 should result in a ban or exclusive selection of the relevant token.
        /// </summary>
        [JsonPropertyName("logit_bias")]
        public Dictionary<string, double>? LogitBias { get; set; }

        /// <summary>
        /// A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse.
        /// </summary>
        [JsonPropertyName("user")]
        public string? User { get; set; }

        /// <summary>
        /// If specified, our system will make a best effort to sample deterministically, such that repeated requests with the same seed and parameters should return the same result.
        /// Determinism is not guaranteed, and you should refer to the system_fingerprint response parameter to monitor changes in the backend.
        /// </summary>
        [JsonPropertyName("seed")]
        public int? Seed { get; set; }

        /// <summary>
        /// ChatRequest string representation
        /// </summary>
        /// <returns>ChatRequest string representation</returns>
        public override string ToString() => Messages?.LastOrDefault()?.ToString() ?? String.Empty;
    }
}
