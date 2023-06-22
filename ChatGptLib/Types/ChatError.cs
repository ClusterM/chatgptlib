﻿using System.Text.Json.Serialization;

namespace wtf.cluster.ChatGptLib.Types
{
    /// <summary>
    /// The error received from the API.
    /// </summary>
    public class ChatError
    {
        /// <summary>
        /// The human-readable error message.
        /// </summary>
        [JsonPropertyName("message")]
        public string? Message { get; init; }

        /// <summary>
        /// The error type.
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; init; }

        /// <summary>
        /// The error parameter.
        /// </summary>
        [JsonPropertyName("param")]
        public string? Param { get; init; }

        /// <summary>
        /// The code of the error.
        /// </summary>
        [JsonPropertyName("code")]
        public string? Code { get; init; }

        /// <summary>
        /// The constructor for internal usage.
        /// </summary>
        /// <param name="message">The human-readable error message.</param>
        /// <param name="type">The rrror type.</param>
        /// <param name="param">The rrror parameter.</param>
        /// <param name="code">The code of the error.</param>
        [JsonConstructor]
        public ChatError(string? message, string? type, string? param, string? code)
        {
            Message = message;
            Type = type;
            Param = param;
            Code = code;
        }
    }

    /// <summary>
    /// Error container (just for deserialization).
    /// </summary>
    public class ChatGptErrorContainer
    {
        [JsonPropertyName("error")]
        public ChatError? Error { get; init; }

        [JsonConstructor]
        public ChatGptErrorContainer(ChatError? error)
        {
            Error = error;
        }
    }
}