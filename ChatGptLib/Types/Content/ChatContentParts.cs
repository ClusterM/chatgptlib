using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace wtf.cluster.ChatGptLib.Types.Content
{
    /// <summary>
    /// Class that represents multipart content
    /// </summary>
    public class ChatContentParts : IChatContent
    {
        /// <summary>
        /// Content parts.
        /// </summary>
        [JsonPropertyName("parts")]
        public IList<IChatContentPart> Parts { get; set; }

        /// <summary>
        /// ChatContentParts constructor.
        /// </summary>
        /// <param name="parts">Content parts.</param>
        public ChatContentParts(IList<IChatContentPart> parts)
        {
            Parts = parts;
        }

        /// <summary>
        /// ChatContentParts constructor.
        /// </summary>
        /// <param name="parts">Content parts.</param>
        public ChatContentParts(params IChatContentPart[] parts)
        {
            Parts = parts;
        }
    }
}
