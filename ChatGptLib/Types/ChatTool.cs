using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace wtf.cluster.ChatGptLib.Types
{
    /// <summary>
    /// Tool description for the API.
    /// </summary>
    public class ChatTool
    {
        /// <summary>
        /// The type of the tool.
        /// </summary>
        public enum ToolType
        {
            /// <summary>
            /// Function tool.
            /// </summary>
            Function
        }

        /// <summary>
        /// The type of the tool. Currently, only function is supported.
        /// </summary>
        [JsonPropertyName("type")]
        public ToolType Type { get; set; }

        /// <summary>
        /// A list of functions the model may generate JSON inputs for.
        /// </summary>
        [JsonPropertyName("function")]
        public ChatFunction? Function { get; set; }

        /// <summary>
        /// ChatTool string representation.
        /// </summary>
        /// <returns>ChatTool string representation.</returns>
        public override string ToString() => $"{Type}: {Function}";
    }
}
