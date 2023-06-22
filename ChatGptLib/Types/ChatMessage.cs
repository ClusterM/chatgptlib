﻿using System.Text.Json.Serialization;

namespace wtf.cluster.ChatGptLib.Types
{
    /// <summary>
    /// Chat message, including role and text (or function call result).
    /// </summary>
    public class ChatMessage
    {
        public enum ChatMessageRole { System, User, Assistant, Function };

        /// <summary>
        /// The role of the messages author. One of system, user, assistant, or function.
        /// </summary>
        [JsonPropertyName("role")]
        public ChatMessageRole? Role { get; set; }

        /// <summary>
        /// The contents of the message. Content is required for all messages except assistant messages with function calls.
        /// </summary>
        [JsonPropertyName("content")]
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)] // Actually, it's always required, must me null for function calls.
        public string? Content { get; set; }

        /// <summary>
        /// The name of the author of this message. Name is required if role is function, and it should be the name of the function whose response is in the content. May contain a-z, A-Z, 0-9, and underscores, with a maximum length of 64 characters.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// The name and the arguments of the function that must be called, as generated by the model.
        /// </summary>
        [JsonPropertyName("function_call")]
        public ChatFunctionCallRequest? FunctionCall { get; set; }

        /// <summary>
        /// The constructor for empty object.
        /// </summary>
        public ChatMessage() { }

        /// <summary>
        /// The constructor with the arguments.
        /// </summary>
        /// <param name="role">The role of the messages author. One of system, user, assistant, or function.</param>
        /// <param name="content">The contents of the message. Content is required for all messages except assistant messages with function calls.</param>
        /// <param name="name">The name of the author of this message. name is required if role is function.</param>
        /// <param name="functionCall">The name and the arguments of the function that must be called, as generated by the model.</param>
        [JsonConstructor]
        public ChatMessage(ChatMessageRole? role, string? content, string? name = null, ChatFunctionCallRequest? functionCall = null)
        {
            Role = role;
            Content = content;
            Name = name;
            FunctionCall = functionCall;
        }

        public static ChatMessage operator +(ChatMessage a, ChatMessage b)
        {
            var n = new ChatMessage
            {
                Role = a.Role ?? b.Role,
                Content = (a.Content == null && b.Content == null) ? null : ((a.Content ?? "") + (b.Content ?? "")),
                Name = b.Name ?? a.Name,
                FunctionCall = (a.FunctionCall, b.FunctionCall) switch
                {
                    (null, null) => null,
                    (null, _) => b.FunctionCall,
                    (_, null) => a.FunctionCall,
                    _ => a.FunctionCall + b.FunctionCall
                }
            };
            return n;
        }

        public override string ToString() => $"{Role}: {Content ?? FunctionCall?.ToString() ?? string.Empty}";
    }
}