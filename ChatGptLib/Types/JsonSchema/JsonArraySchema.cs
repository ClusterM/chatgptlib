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

        /// <summary>
        /// The constructor for an empty object.
        /// </summary>
        public JsonArraySchema() { }

        /// <summary>
        /// JsonArraySchema with the arguments.
        /// </summary>
        /// <param name="description">Description.</param>
        /// <param name="items">Type of the array items as another IJsonSchema.</param>
        /// <param name="minItems">Minimum amount of array items.</param>
        /// <param name="maxItems">Maximum amount of array items.</param>
        /// <param name="uniqueItems">true if every item must be unique.</param>
        [JsonConstructor]
        public JsonArraySchema(string? description, IJsonSchema? items, int? minItems = null, int? maxItems = null, bool? uniqueItems = null)
        {
            Description = description;
            Items = items;
            MinItems = minItems;
            MaxItems = maxItems;
            UniqueItems = uniqueItems;
        }
    }
}
