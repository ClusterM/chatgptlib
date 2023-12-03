using System.Text.Json;
using System.Text.Json.Serialization;

namespace wtf.cluster.ChatGptLib.Types.Content
{
    /// <summary>
    /// Content of a chat message.
    /// </summary>
    public interface IChatContent
    {
        /// <summary>
        /// JSON converter for serialization and deserialization.
        /// </summary>
        public class ChatContentConverter : JsonConverter<IChatContent>
        {
            /// <summary>
            /// IChatContent objects deserializer, unused.
            /// </summary>
            /// <exception cref="NotImplementedException"></exception>
            public override IChatContent? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                // API can return string only (yet?)
                return new ChatContentText(reader.GetString() ?? String.Empty);
            }

            /// <summary>
            /// IChatContent objects serializer.
            /// </summary>
            public override void Write(Utf8JsonWriter writer, IChatContent value, JsonSerializerOptions options)
            {
                if (value is ChatContentText v)
                    JsonSerializer.Serialize(writer, v.Text, typeof(string), options);
                else if (value is ChatContentParts p)
                    JsonSerializer.Serialize(writer, p.Parts, typeof(IList<IChatContentPart>), options);
                else
                    throw new NotFiniteNumberException($"Can't serialize {value.GetType()} object");
            }
        }
    }
}
