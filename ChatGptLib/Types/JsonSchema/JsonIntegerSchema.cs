using System.Text.Json.Serialization;

namespace wtf.cluster.ChatGptLib.Types.JsonSchema
{
    /// <summary>
    /// JSON integer value.
    /// </summary>
    public class JsonIntegerSchema : IJsonSchema
    {
        /// <summary>
        /// Type "integer".
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; } = "integer";

        /// <summary>
        /// Description of the element.
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// Minimum value.
        /// </summary>
        [JsonPropertyName("minimum")]
        public double? Minimum { get; set; }

        /// <summary>
        /// Maximum value.
        /// </summary>
        [JsonPropertyName("maximum")]
        public double? Maximum { get; set; }

        /// <summary>
        /// Numbers can be restricted to a multiple of a given number.
        /// </summary>
        [JsonPropertyName("multipleOf")]
        public double? MultipleOf { get; set; }
    }
}
