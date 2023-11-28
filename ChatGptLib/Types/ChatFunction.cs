using System.Text.Json.Serialization;
using wtf.cluster.ChatGptLib.Types.JsonSchema;

namespace wtf.cluster.ChatGptLib.Types
{
    /// <summary>
    /// Function description for the API.
    /// </summary>
    public class ChatFunction
    {
        /// <summary>
        /// The name of the function to be called. Must be a-z, A-Z, 0-9, or contain underscores and dashes, with a maximum length of 64.
        /// </summary>
        [JsonPropertyName("name")]
        public required string Name { get; set; }

        /// <summary>
        /// The description of what the function does.
        /// </summary>
        [JsonPropertyName("description")]
        public required string Description { get; set; }

        /// <summary>
        /// The parameters the functions accepts, described as a JSON Schema object.
        /// </summary>
        [JsonPropertyName("parameters")]
        public required IJsonSchema Parameters { get; set; }

        /// <summary>
        /// ChatFunction string representation.
        /// </summary>
        /// <returns>ChatFunction string representation.</returns>
        public override string ToString() => $"{Name} ({Description})";
    }
}
