using System.Text.Json.Serialization;

namespace wtf.cluster.ChatGptLib.Types
{
    /// <summary>
    /// Represents a chat completion response returned by model, based on the provided input.
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
        /// This fingerprint represents the backend configuration that the model runs with.
        /// Can be used in conjunction with the seed request parameter to understand when backend changes have been made that might impact determinism.
        /// </summary>
        [JsonPropertyName("system_fingerprint")]
        public string? SystemFinterprint { get; internal set; }

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

        /// <summary>
        /// Constructor of empty ChatResponse object.
        /// </summary>
        public ChatResponse() { }

        /// <summary>
        /// The constructor for internal usage.
        /// </summary>
        /// <param name="id">The identifier of the result, which may be used during troubleshooting.</param>
        /// <param name="systemFinterprint">This fingerprint represents the backend configuration that the model runs with.</param>
        /// <param name="model">The name of the used model.</param>
        /// <param name="object">Type of the object.</param>
        /// <param name="created">Creation timestamp.</param>
        /// <param name="choices">The list of the choices that the user was presented with during the chat interaction.</param>
        /// <param name="usage">The usage statistics for the chat interaction.</param>
        [JsonConstructor]
        public ChatResponse(string? id, string? model, string? systemFinterprint, string? @object, int? created, IReadOnlyList<ChatChoice> choices, ChatUsage? usage = null)
        {
            Id = id;
            Model = model;
            SystemFinterprint = systemFinterprint;
            Object = @object;
            Created = created;
            Choices = choices;
            Usage = usage;
        }

        /// <summary>
        /// + operator
        /// </summary>
        /// <param name="a">First ChatResponse object.</param>
        /// <param name="b">Second ChatResponse object.</param>
        /// <returns>Combined ChatResponse object.</returns>
        public static ChatResponse operator +(ChatResponse a, ChatResponse b)
        {
            var n = new ChatResponse()
            {
                Id = a.Id ?? b.Id,
                Model = a.Model ?? b.Model,
                SystemFinterprint = a.SystemFinterprint ?? b.SystemFinterprint,
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

        /// <summary>
        /// ChatResponse string representation.
        /// </summary>
        /// <returns>ChatResponse string representation.</returns>
        public override string ToString() => Choices != null ? string.Concat(Choices.Select(ch => $"{ch}"), ", ") : String.Empty;
    }
}
