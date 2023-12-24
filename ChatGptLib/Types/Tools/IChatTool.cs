using System.Text.Json;
using System.Text.Json.Serialization;

namespace wtf.cluster.ChatGptLib.Types.Tools
{
    /// <summary>
    /// Tool that GPT can use.
    /// </summary>
    public interface IChatTool
    {
        /// <summary>
        /// Tool types.
        /// </summary>
        public enum ToolType
        {
            /// <summary>
            /// Function tool.
            /// </summary>
            Function
        }

        /// <summary>
        /// Tool type.
        /// </summary>
        [JsonPropertyName("type")]
        public ToolType Type { get; }

        /// <summary>
        /// JSON converter for serialization and deserialization.
        /// </summary>
        public class ChatToolConverter : JsonConverter<IChatTool>
        {
            /// <summary>
            /// IChatTool objects deserializer.
            /// </summary>
            public override IChatTool? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                using (JsonDocument document = JsonDocument.ParseValue(ref reader))
                {
                    JsonElement root = document.RootElement;
                    var t = root.GetProperty("type");
                    switch (t.GetString())
                    {
                        case "function":
                            return root.Deserialize<ChatToolFunction>(options);
                        default:
                            throw new JsonException($"Can't deserialize {typeToConvert} object");
                    }
                }
            }

            /// <summary>
            /// IChatTool objects serializer.
            /// </summary>
            public override void Write(Utf8JsonWriter writer, IChatTool value, JsonSerializerOptions options)
            {
                JsonSerializer.Serialize(writer, value, value.GetType(), options);
            }
        }
    }
}
