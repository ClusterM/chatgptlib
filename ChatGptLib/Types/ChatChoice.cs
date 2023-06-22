using System.Text.Json.Serialization;

namespace wtf.cluster.ChatGptLib.Types
{
    /// <summary>
    /// One of the messages received from the API, including the message/delta block, index, and the reason why the message finished.
    /// </summary>
    public class ChatChoice
    {
        /// <summary>
        /// The index of the choice in the list of choices.
        /// </summary>
        [JsonPropertyName("index")]
        public int Index { get; init; }

        /// <summary>
        /// The message that was presented to the user as the choice.
        /// </summary>
        [JsonPropertyName("message")]
        public ChatMessage? Message { get; init; }

        /// <summary>
        /// The message part for streaming.
        /// </summary>
        [JsonPropertyName("delta")]
        public ChatMessage? Delta { get; init; }

        /// <summary>
        /// The reason why the chat interaction ended after this choice was presented to the user.
        /// </summary>
        [JsonPropertyName("finish_reason")]
        public string? FinishReason { get; init; }

        /// <summary>
        /// The constructor for internal usage.
        /// </summary>
        /// <param name="index">The index of the choice.</param>
        public ChatChoice(int index) 
        {
            Index = index;
        }

        /// <summary>
        /// The constructor for internal usage.
        /// </summary>
        /// <param name="index">The index of the choice in the list of choices.</param>
        /// <param name="message">The message that was presented to the user as the choice.</param>
        /// <param name="delta">The message part for streaming.</param>
        /// <param name="finishReason">The reason why the chat interaction ended after this choice was presented to the user.</param>
        [JsonConstructor]
        public ChatChoice(int index, ChatMessage? message, ChatMessage? delta, string? finishReason)
        {
            Index = index;
            Message = message;
            Delta = delta;
            FinishReason = finishReason;
        }

        public static ChatChoice operator +(ChatChoice a, ChatChoice b)
        {
            if (a.Index != b.Index) 
                throw new InvalidOperationException("Different choice indices");
            var am = a.Message ?? a.Delta;
            var bm = b.Message ?? b.Delta;
            var n = new ChatChoice(a.Index) {
                Message = (am, bm) switch
                {
                    (null, null) => null,
                    (null, _) => bm,
                    (_, null) => am,
                    _ => am + bm
                },
                Delta = null,
                FinishReason = a.FinishReason ?? b.FinishReason
            };
            return n;
        }

        public override string ToString() => Message?.ToString() ?? string.Empty;
    }
}
