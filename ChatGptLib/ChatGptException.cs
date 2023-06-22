using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wtf.cluster.ChatGptLib.Types;

namespace wtf.cluster.ChatGptLib
{
    /// <summary>
    /// Exception is thrown when the API returns an error
    /// </summary>
    public class ChatGptException : Exception
    {
        /// <summary>
        /// Error details
        /// </summary>
        public ChatError ChatError { get; }

        /// <summary>
        /// Main constructor.
        /// </summary>
        /// <param name="error">ChatError object.</param>
        public ChatGptException(ChatError error) : base(error.Message) 
        { 
            ChatError = error;
        }

        /// <summary>
        /// Error humar-readable message.
        /// </summary>
        /// <returns>Error humar-readable message.</returns>
        public override string ToString() => ChatError?.Message ?? string.Empty;
    }
}
