using System.Text.Json.Serialization;
using System.Text.Json;
using wtf.cluster.ChatGptLib.Types.Content;

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
            /// IJsonSchema objects deserializer.
            /// </summary>
            public override IJsonSchema? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                using (JsonDocument document = JsonDocument.ParseValue(ref reader))
                {
                    JsonElement root = document.RootElement;
                    var t = root.GetProperty("type");
                    switch (t.GetString())
                    {
                        case "array":
                            return root.Deserialize<JsonArraySchema>(options);
                        case "boolean":
                            return root.Deserialize<JsonBooleanSchema>(options);
                        case "integer":
                            return root.Deserialize<JsonIntegerSchema>(options);
                        case "number":
                            return root.Deserialize<JsonNumberSchema>(options);
                        case "object":
                            return root.Deserialize<JsonObjectSchema>(options);
                        case "string":
                            return root.Deserialize<JsonStringSchema>(options);
                        default:
                            throw new JsonException($"Can't deserialize {typeToConvert} object");
                    }
                }
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
