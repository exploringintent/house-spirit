using HouseSpirit.Services.OllamaServices;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.TextGeneration;
using OllamaSharp;

namespace HouseSpirit.Services
{
    public class GenerationService : IGenerationService
    {      
        public string GiveChatResponse(string userPrompt)
        {            
            var ollamaChat = new OllamaChatService();
            ollamaChat.ModelUrl = "http://localhost:11434";
            ollamaChat.ModelName = "llama3";

            var builder = Kernel.CreateBuilder();
            builder.Services.AddKeyedSingleton<IChatCompletionService>("ollamaChat", ollamaChat);
            var kernel = builder.Build();
        
            // chat
            var chat = kernel.GetRequiredService<IChatCompletionService>();
            var history = new ChatHistory();
            history.AddSystemMessage("You are a useful assistant that replies using witty and consise responses.");
            history.AddUserMessage(userPrompt);

            // print response
            var result = chat.GetChatMessageContentsAsync(history);
            return result.Result[^1].Content ?? string.Empty;
        }        

        public string GiveCompletionResponse(string userPrompt)
        {   
            var ollamaText = new OllamaGenerationService();
            ollamaText.ModelUrl = "http://localhost:11434";
            ollamaText.ModelName = "llama3";

            var builder = Kernel.CreateBuilder();
            builder.Services.AddKeyedSingleton<ITextGenerationService>("ollamaText", ollamaText);
            var kernel = builder.Build();
          
            // text generation
            var textGen = kernel.GetRequiredService<ITextGenerationService>();
            var response = textGen.GetTextContentsAsync(userPrompt + " ").Result;          
            return response[^1].Text ?? string.Empty;
        }        
    }
}