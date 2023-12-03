using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static wtf.cluster.ChatGptLib.Types.Content.ChatContentPartImageUrl;

namespace wtf.cluster.ChatGptLib.Types.Content
{
    /// <summary>
    /// Url to the image
    /// </summary>
    public class ChatContentPartImageUrl : IChatContentPart
    {
        /// <summary>
        /// Content part type
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; } = "image_url";

        /// <summary>
        /// URL to the image
        /// </summary>
        [JsonPropertyName("image_url")]
        public ImageUrl Image { get; set; }

        /// <summary>
        /// ChatContentPartImageUrl constructor.
        /// </summary>
        /// <param name="imageUrl">Image URL as ImageUrl object.</param>
        /// <exception cref="InvalidDataException"></exception>
        public ChatContentPartImageUrl(ImageUrl imageUrl)
        {
            Image = imageUrl;
        }

        /// <summary>
        /// ChatContentPartImageUrl constructor.
        /// </summary>
        /// <param name="imageUrl">Image URL as string.</param>
        /// <exception cref="InvalidDataException"></exception>
        public ChatContentPartImageUrl(string imageUrl) : this(new ImageUrl(imageUrl)) { }

        /// <summary>
        /// Subclass for image URL
        /// </summary>
        public class ImageUrl
        {
            /// <summary>
            /// Image URL as string.
            /// </summary>
            [JsonPropertyName("url")]
            public string Url { get; set; }

            /// <summary>
            /// ImageUrl constructor.
            /// </summary>
            /// <param name="imageUrl">Image URL as string.</param>
            public ImageUrl(string imageUrl) => Url = imageUrl;
        }
    }
}
