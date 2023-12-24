using System.Text.Json.Serialization;

namespace wtf.cluster.ChatGptLib.Types.Tools
{
    /// <summary>
    /// Function tool description for the API.
    /// </summary>
    public class ChatToolFunction : IChatTool
    {
        /// <summary>
        /// The type of the tool. Currently, only function is supported.
        /// </summary>
        [JsonPropertyName("type")]
        public IChatTool.ToolType Type { get; } = IChatTool.ToolType.Function;

        /// <summary>
        /// A function the model may generate JSON inputs for.
        /// </summary>
        [JsonPropertyName("function")]
        public ChatFunction Function { get; set; }

        /// <summary>
        /// ChatToolFunction constructor.
        /// </summary>
        /// <param name="function">Function as ChatFunction object.</param>
        [JsonConstructor]
        public ChatToolFunction(ChatFunction function)
        {
            Function = function;
        }

        /// <summary>
        /// ChatToolFunction string representation.
        /// </summary>
        /// <returns>ChatTool string representation.</returns>
        public override string ToString() => $"Function: {Function}";
    }
}
