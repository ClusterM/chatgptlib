using System.Text.Json;
using System.Threading;
using wtf.cluster.ChatGptLib.Types.JsonSchema;

namespace wtf.cluster.ChatGptLib.Types
{
    /// <summary>
    /// Represents full function description with callback.
    /// </summary>
    public record ChatFunctionMethod
    {
        /// <summary>
        /// Function callback delegate.
        /// </summary>
        /// <param name="args">Function call arguments as parsed JSON.</param>
        /// <param name="o">Optional object to pass to a function.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Function result as string.</returns>
        public delegate Task<string> Callback(JsonElement args, object? o, CancellationToken cancellationToken = default);

        /// <summary>
        /// Function description.
        /// </summary>
        public required string Description { get; init; }

        /// <summary>
        /// Function callback.
        /// </summary>
        public required Callback Function { get; init; }

        /// <summary>
        /// Parameters schema.
        /// </summary>
        public required IJsonSchema Parameters { get; init; }
    }
}
