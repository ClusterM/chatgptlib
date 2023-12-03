using System.Text.Json.Serialization;

namespace wtf.cluster.ChatGptLib.Types
{
    /// <summary>
    /// The tool call generated by the model, such as function calls.
    /// </summary>
    public class ChatToolCallRequest
    {
        /// <summary>
        /// The ID of the tool call.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; init; }

        /// <summary>
        /// The type of the tool. Currently, only function is supported.
        /// Can be partial when using streaming, so do not parse it until the data has been fully received.
        /// </summary>
        [JsonPropertyName("type")]
        public ChatTool.ToolType? Type { get; init; }

        /// <summary>
        /// The function that the model called.
        /// </summary>
        [JsonPropertyName("function")]
        public ChatFunctionCallRequest? Function { get; init; }

        /// <summary>
        /// The constructor for internal usage.
        /// </summary>
        /// <param name="id">The ID of the tool call.</param>
        /// <param name="type">The type of the tool.</param>
        /// <param name="function">The function that the model called.</param>
        [JsonConstructor]
        public ChatToolCallRequest(string id, ChatTool.ToolType? type, ChatFunctionCallRequest? function)
        {
            Id = id;
            Type = type;
            Function = function;
        }

        /// <summary>
        /// + operator
        /// </summary>
        /// <param name="a"> First ChatToolCallRequest object.</param>
        /// <param name="b"> Second ChatToolCallRequest object.</param>
        /// <returns>Combined ChatToolCallRequest object.</returns>
        public static ChatToolCallRequest operator +(ChatToolCallRequest a, ChatToolCallRequest b)
        {
            var n = new ChatToolCallRequest(
                id: a.Id ?? b.Id,
                type: a.Type ?? b.Type,
                function: (a.Function, b.Function) switch
                {
                    (null, null) => null,
                    (null, _) => b.Function,
                    (_, null) => a.Function,
                    _ => a.Function + b.Function
                }
            );
            return n;
        }

        /// <summary>
        /// ChatToolCallRequest string representation.
        /// </summary>
        /// <returns>ChatToolCallRequest string representation.</returns>
        public override string ToString() => $"{(Type != null ? $"{Type}" : "no type")}: {Function}";
    }
}
