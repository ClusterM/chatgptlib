using System.Text.Json.Serialization;

namespace wtf.cluster.ChatGptLib.Types
{
    /// <summary>
    /// Response received from the API, including the list of choices and the statistics.
    /// </summary>
    public class ChatResponse
    {
        /// <summary>
        /// The identifier of the result, which may be used during troubleshooting.
        /// </summary>
        [JsonPropertyName("id")]
        public string? Id { get; internal set; }

        /// <summary>
        /// The name of the used model.
        /// </summary>
        [JsonPropertyName("model")]
        public string? Model { get; internal set; }

        /// <summary>
        /// Type of the object.
        /// </summary>
        [JsonPropertyName("object")]
        public string? Object { get; internal set; }

        /// <summary>
        /// Creation timestamp.
        /// </summary>
        [JsonPropertyName("created")]
        public int? Created { get; internal set; }

        /// <summary>
        /// The list of the choices that the user was presented with during the chat interaction.
        /// </summary>
        [JsonPropertyName("choices")]
        public IReadOnlyList<ChatChoice> Choices { get; internal set; } = new List<ChatChoice>();

        /// <summary>
        /// The usage statistics for the chat interaction.
        /// </summary>
        [JsonPropertyName("usage")]
        public ChatUsage? Usage { get; internal set; }

        // Create empty ChatResponse object
        public ChatResponse() { }

        /// <summary>
        /// The constructor for internal usage.
        /// </summary>
        /// <param name="id">The identifier of the result, which may be used during troubleshooting.</param>
        /// <param name="model">The name of the used model.</param>
        /// <param name="object">Type of the object.</param>
        /// <param name="created">Creation timestamp.</param>
        /// <param name="choices">The list of the choices that the user was presented with during the chat interaction.</param>
        /// <param name="usage">The usage statistics for the chat interaction.</param>
        [JsonConstructor]
        public ChatResponse(string? id, string? model, string? @object, int? created, IReadOnlyList<ChatChoice> choices, ChatUsage? usage)
        {
            Id = id;
            Model = model;
            Object = @object;
            Created = created;
            Choices = choices;
            Usage = usage;
        }

        public static ChatResponse operator +(ChatResponse a, ChatResponse b)
        {
            var n = new ChatResponse()
            {
                Id = a.Id ?? b.Id,
                Model = a.Model ?? b.Model,
                Object = a.Object ?? b.Object,
                Created = a.Created ?? b.Created,
                Usage = a.Usage ?? b.Usage
            };
            var list = new List<ChatChoice>();
            while (a.Choices.Any() && list.Count() < a.Choices.Max(i => i.Index) + 1)
                list.Add(new ChatChoice(n.Choices.Count));
            while (b.Choices.Any() && list.Count() < b.Choices.Max(i => i.Index) + 1)
                list.Add(new ChatChoice(n.Choices.Count));
            foreach (var choice in a.Choices)
            {
                list[choice.Index] = choice;
            }
            foreach (var choice in b.Choices)
            {
                list[choice.Index] += choice;
            }
            n.Choices = list.AsReadOnly();
            return n;
        }

        public override string ToString() => Choices?.FirstOrDefault()?.ToString() ?? string.Empty;
    }
}
