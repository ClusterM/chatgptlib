namespace wtf.cluster.ChatGptLib.Types.JsonSchema
{
    /// <summary>
    /// Interface for a element type description.
    /// </summary>
    public interface IJsonSchema
    {
        string Type { get; }
        string? Description { get; set; }
    }
}
