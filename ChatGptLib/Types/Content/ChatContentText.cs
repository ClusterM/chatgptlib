using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace wtf.cluster.ChatGptLib.Types.Content
{
    /// <summary>
    /// Content that represents text string.
    /// </summary>
    public class ChatContentText : IChatContent
    {
        /// <summary>
        /// Text string.
        /// </summary>
        [JsonPropertyName("type")]
        public string Text { get; set; }

        /// <summary>
        /// ChatContentText constructor.
        /// </summary>
        /// <param name="text">Text string</param>
        public ChatContentText(string text)
        {
            Text = text;
        }

        /// <summary>
        /// + operator
        /// </summary>
        /// <param name="a">First ChatContentText object.</param>
        /// <param name="b">Second ChatContentText object.</param>
        /// <returns>Combined ChatContentText object.</returns>
        public static ChatContentText operator +(ChatContentText a, ChatContentText b)
        {
            return new ChatContentText(a.Text + b.Text);
        }

        /// <summary>
        /// Explicit ChatContentText conversion to string.
        /// </summary>
        /// <param name="t">ChatContentText</param>
        public static explicit operator string(ChatContentText t) => t.Text;

        /// <summary>
        /// Explicit string conversion to ChatContentText.
        /// </summary>
        /// <param name="t">ChatContentText</param>
        public static explicit operator ChatContentText(string t) => new ChatContentText(t);

        /// <summary>
        /// ChatContentText string representation.
        /// </summary>
        /// <returns>ChatContentText string representation.</returns>
        public override string ToString() => Text;
    }
}
