using System.Text.Json.Serialization;
using System.Text.Json;

namespace wtf.cluster.ChatGptLib.Types.JsonSchema
{
    /// <summary>
    /// Interface for a element type description.
    /// </summary>
    public interface IJsonSchema
    {
        /// <summary>
        /// Variable type.
        /// </summary>
        string Type { get; }

        /// <summary>
        /// Variable description.
        /// </summary>
        string? Description { get; set; }

        /// <summary>
        /// JSON converter for serialization and deserialization.
        /// </summary>
        public class JsonSchemaConverter : JsonConverter<IJsonSchema>
        {
            /// <summary>
            /// IJsonSchema objects deserializer, unused.
            /// </summary>
            /// <exception cref="NotImplementedException"></exception>
            public override IJsonSchema? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// IJsonSchema objects serializer.
            /// </summary>
            public override void Write(Utf8JsonWriter writer, IJsonSchema value, JsonSerializerOptions options)
            {
                JsonSerializer.Serialize(writer, value, value.GetType(), options);
            }
        }
    }
}
