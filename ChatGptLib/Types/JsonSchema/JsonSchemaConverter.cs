using System.Text.Json;
using System.Text.Json.Serialization;

namespace wtf.cluster.ChatGptLib.Types.JsonSchema
{
    /// <summary>
    /// JSON converter for serialization and deserialization.
    /// </summary>
    public class JsonSchemaConverter : JsonConverter<IJsonSchema>
    {
        /// <summary>
        /// IJsonSchema objects deserializer, unused
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
