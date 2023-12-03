using System.Text.Json.Serialization;

namespace wtf.cluster.ChatGptLib.Types.JsonSchema
{
    /// <summary>
    /// JSON boolean value.
    /// </summary>
    public class JsonBooleanSchema : IJsonSchema
    {
        /// <summary>
        /// Type "boolean".
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; } = "boolean";

        /// <summary>
        /// Description of the element.
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }
}
