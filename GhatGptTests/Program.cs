using System.Text.Json;
using System.Web;
using wtf.cluster.ChatGptLib;
using wtf.cluster.ChatGptLib.Types;
using wtf.cluster.ChatGptLib.Types.Content;
using wtf.cluster.ChatGptLib.Types.JsonSchema;
using static wtf.cluster.ChatGptLib.Types.ChatMessage;

namespace GhatGptTests
{
    internal class Program
    {
        const string OpenAIKey = "<insert your OpenAI API key here>";
        const string YouTubeApiKey = "<insert your YouTube API key here>";

        const string RequestFunctionsText = "Please find Rick Roll video on YouTube and give me the link to the largest preview image. Also, say what time is it now.";
        const string RequestImageText = "What’s in this image?";
        const string ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/d/d6/Oreo-Size-Variations.jpg/2880px-Oreo-Size-Variations.jpg";

        static async Task Main()
        {
            await FunctionsTest();
            Console.WriteLine();
            await ImageTest();
        }

        static async Task FunctionsTest()
        {
            var messages = new List<ChatMessage>()
            {
                new ChatMessage(ChatMessageRole.User, RequestFunctionsText)
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
                        Required = new string[] { "part" }
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
                    Model = "gpt-4-1106-preview",
                    Messages = messages,
                    Tools = new List<ChatTool>(functions.Select(kv => new ChatTool() { Type = ChatTool.ToolType.Function, Function = new ChatFunction() { Name = kv.Key, Description = kv.Value.Description, Parameters = kv.Value.Parameters } })),
                    Temperature = 0.5
                };
                // Create empty response
                var completionResult = new ChatResponse();
                var completionResultStream = gptApi.RequestStreamAsync(request);
                // Process stream data
                await foreach (var p in completionResultStream)
                {
                    // You can just use + operator to combine stream chunks
                    completionResult = completionResult! + p;
                    // Skip if it's function call
                    if (completionResult?.Choices?.First()?.Message?.ToolCalls != null)
                        continue;
                    // Print partial data
                    if (p.Choices?.FirstOrDefault()?.Delta != null)
                        Console.Write(p.Choices.First().Delta!.Content);
                }
                // Append message to the chat history
                var msg = completionResult!.Choices.FirstOrDefault()?.Message!;
                messages.Add(msg);
                if (msg.ToolCalls != null && msg.ToolCalls.Any())
                {
                    foreach (var tool in msg.ToolCalls)
                    {
                        switch (tool.Type)
                        {
                            case ChatTool.ToolType.Function:
                                // It's function call
                                Console.WriteLine($"Function call: {tool.Function?.Name}");
                                if (!functions.TryGetValue(tool.Function!.Name!, out GptFunction? function))
                                    throw new NotImplementedException($"Unknown function: {tool.Function!.Name}");
                                // calling the fuctions
                                var argsDoc = JsonDocument.Parse(tool.Function!.Arguments!);
                                var functionResult = await function!.Function(argsDoc.RootElement);
                                //Console.WriteLine($"Function call result: {functionResult}");
                                // Need to add function result to the chat history
                                var functionResultMessage = new ChatMessage(ChatMessageRole.Tool, functionResult, tool.Function?.Name)
                                {
                                    ToolCallId = tool.Id
                                };
                                messages.Add(functionResultMessage);
                                break;
                            default:
                                throw new NotImplementedException($"Unknown tool type: {tool.Type}");
                        }
                    }
                }
                else
                {
                    // Text answer received, done.
                    Console.WriteLine();
                    break;
                }
            }

        }

        static async Task ImageTest()
        {
            var messages = new List<ChatMessage>()
            {
                new ChatMessage(ChatMessageRole.User, new ChatContentParts(
                    new List<IChatContentPart>()
                    {
                        new ChatContentPartText(RequestImageText),
                        new ChatContentPartImageUrl(ImageUrl)
                    }
                ))
            };
            var request = new ChatRequest()
            {
                Model = "gpt-4-vision-preview",
                Messages = messages,
                Temperature = 0,
                MaxTokens = 4096
            };
            var gptApi = new ChatGptClient(OpenAIKey);
            var completionResult = new ChatResponse();
            var completionResultStream = gptApi.RequestStreamAsync(request);
            // Process stream data
            await foreach (var p in completionResultStream)
            {
                // Print partial data
                if (p.Choices?.FirstOrDefault()?.Delta != null)
                    Console.Write(p.Choices.First().Delta!.Content);
            }
            Console.WriteLine();
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

            public required string Description { get; init; }
            public required GptFunctionMethod Function { get; init; }
            public required IJsonSchema Parameters { get; init; }
        }
    }
}
