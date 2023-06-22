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

        public ChatGptException(ChatError error) : base(error.Message) 
        { 
            ChatError = error;
        }

        public override string ToString() => ChatError?.Message ?? string.Empty;
    }
}
