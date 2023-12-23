using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace wtf.cluster.ChatGptLib.Types.Content
{
    /// <summary>
    /// Chat message content.
    /// </summary>
    public interface IChatContentPart
    {
        /// <summary>
        /// Content part type.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; }

        /// <summary>
        /// JSON converter for serialization and deserialization.
        /// </summary>
        public class ChatContentPartConverter : JsonConverter<IChatContentPart>
        {
            /// <summary>
            /// IChatContentPart objects deserializer, unused.
            /// </summary>
            public override IChatContentPart? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                using (JsonDocument document = JsonDocument.ParseValue(ref reader))
                {
                    JsonElement root = document.RootElement;
                    var t = root.GetProperty("type");
                    switch (t.GetString())
                    {
                        case "text":
                            return root.Deserialize<ChatContentPartText>(options);
                        case "image_url":
                            return root.Deserialize<ChatContentPartImageUrl>(options);
                        default:
                            throw new JsonException($"Can't deserialize {typeToConvert} object");
                    }
                }
            }

            /// <summary>
            /// IChatContentPart objects serializer.
            /// </summary>
            public override void Write(Utf8JsonWriter writer, IChatContentPart value, JsonSerializerOptions options)
            {
                JsonSerializer.Serialize(writer, value, value.GetType(), options);
            }
        }
    }
}
