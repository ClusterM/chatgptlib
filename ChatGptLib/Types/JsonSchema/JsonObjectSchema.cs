using System.Text.Json.Serialization;

namespace wtf.cluster.ChatGptLib.Types.JsonSchema
{
    /// <summary>
    /// JSON object value.
    /// </summary>
    public class JsonObjectSchema : IJsonSchema
    {
        /// <summary>
        /// Type "object".
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; } = "object";

        /// <summary>
        /// Description of the element.
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// Dictionary of the object property names and types.
        /// </summary>
        [JsonPropertyName("properties")]
        public Dictionary<string, IJsonSchema> Properties { get; set; } = new();

        /// <summary>
        /// List of the required fields of the object.
        /// </summary>
        [JsonPropertyName("required")]
        public IList<string>? Required { get; set; }

        /// <summary>
        /// The constructor for an empty object.
        /// </summary>
        public JsonObjectSchema() { }

        /// <summary>
        /// JsonObjectSchema constructor with the arguments.
        /// </summary>
        /// <param name="description">Description</param>
        /// <param name="properties">Properties as name-schema dictionary.</param>
        /// <param name="required">List of required properties.</param>
        [JsonConstructor]
        public JsonObjectSchema(string? description, Dictionary<string, IJsonSchema>? properties, IList<string>? required = null)
        {
            Description = description;
            Properties = properties ?? new();
            Required = required;
        }
    }
}
