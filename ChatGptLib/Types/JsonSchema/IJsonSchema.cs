namespace wtf.cluster.ChatGptLib.Types.JsonSchema
{
    /// <summary>
    /// Interface for a element type description.
    /// </summary>
    public interface IJsonSchema
    {
        /// <summary>
        /// Variable type
        /// </summary>
        string Type { get; }

        /// <summary>
        /// Variable description
        /// </summary>
        string? Description { get; set; }
    }
}
