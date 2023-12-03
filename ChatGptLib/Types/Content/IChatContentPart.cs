using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using wtf.cluster.ChatGptLib.Types.JsonSchema;

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
            /// <exception cref="NotImplementedException"></exception>
            public override IChatContentPart? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                throw new NotImplementedException();
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
