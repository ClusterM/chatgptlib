using System.Text.Json;
using System.Web;
using wtf.cluster.ChatGptLib;
using wtf.cluster.ChatGptLib.Types;
using wtf.cluster.ChatGptLib.Types.JsonSchema;
using static wtf.cluster.ChatGptLib.Types.ChatMessage;

namespace GhatGptTests
{
    internal class Program
    {
        const string OpenAIKey = "<insert your OpenAI API key here>";
        const string YouTubeApiKey = "<insert your YouTube API key here>";

        //const string RequestText = "How much o'clock? Also, can you say the current date? And day of the week.";
        const string RequestText = "Please find Rick Roll video on YouTube and give me the link to the largest preview image.";
        //const string RequestText = "Please find the most popular video on YouTube.";        

        static async Task Main(string[] args)
        {
            var messages = new List<ChatMessage>()
            {
                new ChatMessage() { Role = ChatMessage.ChatMessageRole.User, Content = RequestText }
            };
            var functions = new Dictionary<string, GptFunction>
            {
                ["youtube_search"] = new GptFunction()
                {
                    Description = "Request to https://www.googleapis.com/youtube/v3/search for YouTube search.",
                    Parameters = new JsonObjectSchema()
                    {
                        Description = "API request parameters.",
                        Properties = new Dictionary<string, IJsonSchema>
                        {
                            // GPT already knows parameter description
                            ["part"] = new JsonStringSchema(),
                            ["channelId"] = new JsonStringSchema(),
                            ["channelType"] = new JsonStringSchema(),
                            ["eventType"] = new JsonStringSchema(),
                            ["maxResults"] = new JsonNumberSchema(),
                            ["order"] = new JsonStringSchema(),
                            ["pageToken"] = new JsonStringSchema(),
                            ["publishedAfter"] = new JsonStringSchema(),
                            ["publishedBefore"] = new JsonStringSchema(),
                            ["q"] = new JsonStringSchema(),
                            ["regionCode"] = new JsonStringSchema(),
                            ["relevanceLanguage"] = new JsonStringSchema(),
                            ["type"] = new JsonStringSchema(),
                            ["videoCaption"] = new JsonStringSchema(),
                            ["videoCategoryId"] = new JsonStringSchema(),
                            ["videoDefinition"] = new JsonStringSchema(),
                            ["videoDimension"] = new JsonStringSchema(),
                            ["videoDuration"] = new JsonStringSchema(),
                            ["videoEmbeddable"] = new JsonStringSchema(),
                            ["videoLicense"] = new JsonStringSchema(),
                            ["videoSyndicated"] = new JsonStringSchema(),
                            ["videoType"] = new JsonStringSchema(),
                        },
                        Required = new List<string> { "part" }
                    },
                    Function = YouTubeSearch
                },
                ["get_time"] = new GptFunction()
                {
                    Description = "Returns current time in ISO 8601 format",
                    Function = (args) => Task.FromResult(DateTime.Now.ToString("o")),
                    Parameters = new JsonObjectSchema()
                }
            };

            var gptApi = new ChatGptClient(OpenAIKey);
            while (true)
            {
                var request = new ChatRequest()
                {
                    Model = "gpt-4-0613",
                    Messages = messages,
                    Functions = new List<ChatFunction>(functions.Select(kv => new ChatFunction() { Name = kv.Key, Description = kv.Value.Description, Parameters = kv.Value.Parameters })),
                    Temperature = 0.5
                };
                // Create empty response
                var completionResult = new ChatResponse();
                var completionResultStream = gptApi.RequestStreamAsync(request);
                // Process stream data
                await foreach (var p in completionResultStream)
                {
                    // Append received data to response object
                    completionResult = completionResult! + p;
                    // Skip if it's function call
                    if (completionResult?.Choices?.First()?.Message?.FunctionCall != null)
                        continue;
                    // Print partial data
                    if (p.Choices?.FirstOrDefault()?.Delta != null)
                        Console.Write(p.Choices.First().Delta!.Content);
                }
                if (completionResult?.Choices?.First()?.Message?.FunctionCall == null)
                    Console.WriteLine();
                else
                    Console.WriteLine($"Function call: {completionResult?.Choices?.First()?.Message?.FunctionCall}");
                // Append message to the chat history
                var msg = completionResult!.Choices.FirstOrDefault()?.Message!;
                messages.Add(msg);
                if (msg.FunctionCall?.Name != null)
                {
                    // It's function call
                    if (!functions.TryGetValue(msg.FunctionCall.Name, out GptFunction? function))
                        throw new NotImplementedException($"Unknown function: {msg.FunctionCall.Name}");
                    // calling the fuctions
                    var argsDoc = JsonDocument.Parse(msg.FunctionCall.Arguments!);
                    var functionResult = await function!.Function(argsDoc.RootElement);
                    Console.WriteLine($"Function call result: {functionResult}");

                    // Need to add function result to the chat history
                    var functionResultMessage = new ChatMessage(ChatMessageRole.Function, functionResult, msg.FunctionCall.Name);
                    messages.Add(functionResultMessage);
                }
                else
                {
                    // Text answer received, done.
                    break;
                }
            }
        }

        static async Task<string> YouTubeSearch(JsonElement args)
        {
            var p = HttpUtility.ParseQueryString(string.Empty);
            foreach (var o in args.EnumerateObject())
            {
                p[o.Name] = $"{o.Value}";
            }
            p["key"] = YouTubeApiKey;
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://www.googleapis.com/youtube/v3/search?{p}")
            };
            using HttpClient client = new HttpClient();
            using var httpResponseMessage = await client.SendAsync(httpRequestMessage).ConfigureAwait(false);
            var json = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            return json;
        }

        class GptFunction
        {
            public delegate Task<string> GptFunctionMethod(JsonElement args);

            public required string? Description { get; init; }
            public required GptFunctionMethod? Function { get; init; }
            public required IJsonSchema? Parameters { get; init; }

            public GptFunction() { }

            public GptFunction(string description, GptFunctionMethod function, IJsonSchema args)
            {
                Description = description;
                Function = function;
                Parameters = args;
            }
        }
    }
}
