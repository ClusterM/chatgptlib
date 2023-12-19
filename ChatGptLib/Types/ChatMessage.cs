using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using wtf.cluster.ChatGptLib.Types.Content;

namespace wtf.cluster.ChatGptLib.Types
{
    /// <summary>
    /// Chat message, including role and text (or function call result).
    /// </summary>
    public class ChatMessage
    {
        /// <summary>
        /// Message role.
        /// </summary>
        public enum ChatMessageRole
        {
            /// <summary>
            /// System message (main rules).
            /// </summary>
            System,
            /// <summary>
            /// Message from user (request from the user).
            /// </summary>
            User,
            /// <summary>
            /// Assistant message (reply from the assistant).
            /// </summary>
            Assistant,
            /// <summary>
            /// Function message (the function execution result).
            /// </summary>
            [Obsolete("Use 'Tool'")]
            Function,
            /// <summary>
            /// Tool message (the tool execution result).
            /// </summary>
            Tool
        };

        /// <summary>
        /// The role of the messages author. One of system, user, assistant, or function.
        /// </summary>
        [JsonPropertyName("role")]
        public ChatMessageRole? Role { get; set; }

        /// <summary>
        /// The contents of the chunk message.
        /// </summary>
        [JsonPropertyName("content")]
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)] // Actually, it's always required, must me null for function calls.
        public IChatContent? Content { get; set; }

        /// <summary>
        /// The name of the author of this message. Name is required if role is function, and it should be the name of the function whose response is in the content. May contain a-z, A-Z, 0-9, and underscores, with a maximum length of 64 characters.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// The name and the arguments of the function that must be called, as generated by the model.
        /// </summary>
        [JsonPropertyName("function_call")]
        [Obsolete("Use 'ToolCalls'")]
        public ChatFunctionCallRequest? FunctionCall { get; set; }

        /// <summary>
        /// A list of tools the model may call. Currently, only functions are supported as a tool.
        /// </summary>
        [JsonPropertyName("tool_calls")]
        public IList<ChatToolCallRequest>? ToolCalls { get; set; }

        /// <summary>
        /// Tool call that this message is responding to.
        /// </summary>
        [JsonPropertyName("tool_call_id")]
        public string? ToolCallId { get; set; }

        /// <summary>
        /// The constructor for empty object.
        /// </summary>
        public ChatMessage() { }

        /// <summary>
        /// ChatMessage constructor with the arguments.
        /// </summary>
        /// <param name="role">The role of the messages author. One of system, user, assistant, or function.</param>
        /// <param name="content">The contents of the message. Content is required for all messages except assistant messages with function calls.</param>
        /// <param name="name">The name of the author of this message. Required if role is function.</param>
        /// <param name="functionCall">The name and the arguments of the function that must be called, as generated by the model.</param>
        /// <param name="toolCalls">The tool calls generated by the model, such as function calls.</param>
        [JsonConstructor]
        public ChatMessage(ChatMessageRole? role, IChatContent? content, string? name = null, ChatFunctionCallRequest? functionCall = null, IList<ChatToolCallRequest>? toolCalls = null)
        {
            Role = role;
            Content = content;
            Name = name;
#pragma warning disable CS0618 // Type or member is obsolete
            FunctionCall = functionCall;
#pragma warning restore CS0618 // Type or member is obsolete
            ToolCalls = toolCalls;
        }

        /// <summary>
        /// ChatMessage constructor with the arguments.
        /// </summary>
        /// <param name="role">The role of the messages author. One of system, user, assistant, or function.</param>
        /// <param name="content">The contents of the message. Content is required for all messages except assistant messages with function calls.</param>
        /// <param name="name">The name of the author of this message. Required if role is function.</param>
        /// <param name="functionCall">The name and the arguments of the function that must be called, as generated by the model.</param>
        /// <param name="toolCalls">The tool calls generated by the model, such as function calls.</param>
        public ChatMessage(ChatMessageRole? role, string content, string? name = null, ChatFunctionCallRequest? functionCall = null, IList<ChatToolCallRequest>? toolCalls = null)
        {
            Role = role;
            Content = new ChatContentText(content);
            Name = name;
#pragma warning disable CS0618 // Type or member is obsolete
            FunctionCall = functionCall;
#pragma warning restore CS0618 // Type or member is obsolete
            ToolCalls = toolCalls;
        }

        /// <summary>
        /// + operator
        /// </summary>
        /// <param name="a">First ChatMessage object.</param>
        /// <param name="b">Second ChatMessage object.</param>
        /// <returns>Combined ChatMessage object.</returns>
        public static ChatMessage operator +(ChatMessage a, ChatMessage b)
        {
            Dictionary<string, ChatToolCallRequest>? toolsSumm = null;
            if (a.ToolCalls != null)
            {
                toolsSumm = new();
                foreach (var tool in a.ToolCalls)
                {
                    if (tool.Id != null)
                        toolsSumm[tool.Id] = tool;
                }
            }
            if (b.ToolCalls != null)
            {
                toolsSumm ??= new();
                foreach (var tool in b.ToolCalls)
                {
                    if (tool.Id == null)
                        toolsSumm[toolsSumm.Last().Key] += tool;
                    else if (toolsSumm.ContainsKey(tool.Id))
                        toolsSumm[tool.Id] += tool;
                    else
                        toolsSumm[tool.Id] = tool;
                }
            }
            var n = new ChatMessage
            {
                Role = a.Role ?? b.Role,
                Content = (a.Content is ChatContentText at && b.Content is ChatContentText bt)
                    ? (at + bt)
                    : a.Content ?? b.Content,
                Name = b.Name ?? a.Name,
#pragma warning disable CS0618 // Type or member is obsolete
                FunctionCall = (a.FunctionCall, b.FunctionCall) switch
                {
                    (null, null) => null,
                    (null, _) => b.FunctionCall,
                    (_, null) => a.FunctionCall,
                    _ => a.FunctionCall + b.FunctionCall
                },
#pragma warning restore CS0618 // Type or member is obsolete
                ToolCalls = toolsSumm?.Select(kv => kv.Value)?.ToList()
            };
            return n;
        }

        /// <summary>
        /// ChatMessage string representation.
        /// </summary>
        /// <returns>ChatMessage string representation.</returns>
#pragma warning disable CS0618 // Type or member is obsolete
        public override string ToString() => $"{Role?.ToString() ?? "no role"}: \"{(
            Content != null ? Content
            : ToolCalls != null
            ? String.Join(", ", ToolCalls.Select(t => t.ToString()))
            : FunctionCall?.ToString()
        )}\"";
#pragma warning restore CS0618 // Type or member is obsolete
    }
}
