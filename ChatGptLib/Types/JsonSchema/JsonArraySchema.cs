using System.Text.Json.Serialization;

namespace wtf.cluster.ChatGptLib.Types.JsonSchema
{
    /// <summary>
    /// JSON array.
    /// </summary>
    public class JsonArraySchema : IJsonSchema
    {
        /// <summary>
        /// Type "array".
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; } = "array";

        /// <summary>
        /// Description of the element.
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// Type of the array elements.
        /// </summary>
        [JsonPropertyName("items")]
        public IJsonSchema? Items { get; set; }

        /// <summary>
        /// Minimum number of items in the array.
        /// </summary>
        [JsonPropertyName("minItems")]
        public int? MinItems { get; set; }

        /// <summary>
        /// Maximum number of items in the array.
        /// </summary>
        [JsonPropertyName("maxItems")]
        public int? MaxItems { get; set; }

        /// <summary>
        /// True if items must be unique in the array.
        /// </summary>
        [JsonPropertyName("uniqueItems")]        
        public bool? UniqueItems { get; set; }
    }
}
