using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using OllamaSharp;
using OllamaSharp.Models.Chat;

namespace HouseSpirit.Services.OllamaServices
{
    public class OllamaChatService : IChatCompletionService
    {
        public string ModelUrl { get; set; }
        public string ModelName { get; set; }

        public IReadOnlyDictionary<string, object?> Attributes => throw new NotImplementedException();

        public OllamaChatService(string uri = "http://localhost:11434", string modelName = "llama3")
        {
            ModelUrl = uri;
            ModelName = modelName;
        }

        public async Task<IReadOnlyList<ChatMessageContent>> GetChatMessageContentsAsync(ChatHistory chatHistory, PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, CancellationToken cancellationToken = default)
        {
            var ollama = new OllamaApiClient(ModelUrl, ModelName);

            var chat = new Chat(ollama, _ => { });


            // iterate though chatHistory Messages
            foreach (var message in chatHistory)
            {
                if (message.Role == AuthorRole.System)
                {
                    await chat.SendAs(ChatRole.System, message.Content);
                    continue;
                }
            }

            var lastMessage = chatHistory.LastOrDefault();

            string question = lastMessage.Content;
            var chatResponse = "";
            var history = (await chat.Send(question, CancellationToken.None)).ToArray();

            var last = history.Last();
            chatResponse = last.Content;

            chatHistory.AddAssistantMessage(chatResponse);

            return chatHistory;
        }

        public IAsyncEnumerable<StreamingChatMessageContent> GetStreamingChatMessageContentsAsync(ChatHistory chatHistory, PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

    }
}