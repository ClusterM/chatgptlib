using System.Text.Json.Serialization;

namespace wtf.cluster.ChatGptLib.Types.JsonSchema
{
    /// <summary>
    /// JSON number value.
    /// </summary>
    public class JsonNumberSchema : IJsonSchema
    {
        /// <summary>
        /// Type "number".
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; } = "number";

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

        /// <summary>
        /// A flag indicating whether the value can not equal the number defined by the maximum attribute.
        /// </summary>
        [JsonPropertyName("exclusiveMaximum")]
        public bool? ExclusiveMaximum { get; set; }

        /// <summary>
        /// A flag indicating whether the value can not equal the number defined by the minimum attribute.
        /// </summary>
        [JsonPropertyName("exclusiveMinimum")]
        public bool? ExclusiveMinimum { get; set; }
    }
}
