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

        /// <summary>
        /// The constructor for an empty object.
        /// </summary>
        public JsonNumberSchema() { }

        /// <summary>
        /// JsonNumberSchema constructor with the arguments.
        /// </summary>
        /// <param name="description">Description.</param>
        /// <param name="minimum">Minimum value.</param>
        /// <param name="maximum">Maximum value.</param>
        /// <param name="multipleOf">Numbers can be restricted to a multiple of a given number.</param>
        /// <param name="exclusiveMaximum">Exclusive maximum value.</param>
        /// <param name="exclusiveMinimum">Exclusive minimum value.</param>
        [JsonConstructor]
        public JsonNumberSchema(string? description, double? minimum = null, double? maximum = null, double? multipleOf = null, bool? exclusiveMaximum = null, bool? exclusiveMinimum = null)
        {
            Description = description;
            Minimum = minimum;
            Maximum = maximum;
            MultipleOf = multipleOf;
            ExclusiveMaximum = exclusiveMaximum;
            ExclusiveMinimum = exclusiveMinimum;
        }

    }
}
