using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace wtf.cluster.ChatGptLib.Types.Content
{
    /// <summary>
    /// Content part that represents text string.
    /// </summary>
    public class ChatContentPartText : IChatContentPart
    {
        /// <summary>
        /// Content part type.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; } = "text";

        /// <summary>
        /// Text string.
        /// </summary>
        [JsonPropertyName("text")]
        public string Text { get; set; }

        /// <summary>
        /// ChatContentPartText constructor.
        /// </summary>
        /// <param name="text">Text string.</param>
        public ChatContentPartText(string text)
        {
            Text = text;
        }

        /// <summary>
        /// + operator
        /// </summary>
        /// <param name="a">First ChatContentPartText object.</param>
        /// <param name="b">Second ChatContentPartText object.</param>
        /// <returns>Combined ChatContentPartText object.</returns>
        public static ChatContentPartText operator +(ChatContentPartText a, ChatContentPartText b)
        {
            return new ChatContentPartText(a.Text + b.Text);
        }

        /// <summary>
        /// Explicit convertion to string.
        /// </summary>
        /// <param name="t"></param>
        public static explicit operator string(ChatContentPartText t) => t.Text;

        /// <summary>
        /// ChatContentPartText string representation.
        /// </summary>
        /// <returns>ChatContentPartText string representation.</returns>
        public override string ToString() => Text;
    }
}