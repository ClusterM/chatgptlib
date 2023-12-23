using System.Text.Json.Serialization;

namespace wtf.cluster.ChatGptLib.Types.JsonSchema
{
    /// <summary>
    /// JSON string value.
    /// </summary>
    public class JsonStringSchema : IJsonSchema
    {
        /// <summary>
        /// Type "string".
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; } = "string";

        /// <summary>
        /// Description of the element.
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// If the field is present, the string must match one of the listed values.
        /// </summary>
        [JsonPropertyName("enum")]
        public List<string>? Enum { get; set; }

        /// <summary>
        /// If the field is present, the string must match the regular expression specified in this field.
        /// </summary>
        [JsonPropertyName("pattern")]
        public string? Pattern { get; set; }

        /// <summary>
        /// Minimum length of the string.
        /// </summary>
        [JsonPropertyName("minLength")]
        public int? MinLength { get; set; }

        /// <summary>
        /// Maximum length of the string.
        /// </summary>
        [JsonPropertyName("maxLength")]
        public int? MaxLength { get; set; }

        /// <summary>
        /// The constructor for an empty object.
        /// </summary>
        public JsonStringSchema() { }

        /// <summary>
        /// JsonStringSchema constructor with the arguments.
        /// </summary>
        /// <param name="description">Description</param>
        /// <param name="enum">Optional list of possible values.</param>
        /// <param name="pattern">Optional regex pattern.</param>
        /// <param name="minLength">Minimum string length.</param>
        /// <param name="maxLength">Maximum string length.</param>
        [JsonConstructor]
        public JsonStringSchema(string? description, List<string>? @enum = null, string? pattern = null, int? minLength = null, int? maxLength = null)
        {
            Description = description;
            Enum = @enum;
            Pattern = pattern;
            MinLength = minLength;
            MaxLength = maxLength;
        }
    }
}
