# ChatGptLib
Just another OpenAI client library (chat completion only) with new functions feature support. For .NET 7.

Full documentation: https://clusterm.github.io/chatgptlib/

## Simple usage example
```C#
internal class Program
{
    const string OpenAIKey = "<insert your OpenAI API key here>";
    const string RequestText = "How much o'clock? Also, can you say the current date? And day of the week.";
    
    static async Task Main(string[] args)
    {
        var messages = new List<ChatMessage>()
        {
            new ChatMessage() { Role = ChatMessage.ChatMessageRole.User, Content = RequestText }
        };
        var functions = new Dictionary<string, GptFunction>
        {                
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
                // You can just use + operator to combine stream chunks
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

    class GptFunction
    {
        public delegate Task<string> GptFunctionMethod(JsonElement args);

        class GptFunction
        {
            public delegate Task<string> GptFunctionMethod(JsonElement args);

            public required string Description { get; init; }
            public required GptFunctionMethod Function { get; init; }
            public required IJsonSchema Parameters { get; init; }
        }    
    }
}
```

Also, check [GhatGptTests](GhatGptTests) directory, there is YouTube search demo.
