using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.TextGeneration;
using OllamaSharp;

namespace HouseSpirit.Services.OllamaServices
{
       public class OllamaGenerationService : ITextGenerationService
    {
        public string ModelUrl { get; set; }
        public string ModelName { get; set; }

        public IReadOnlyDictionary<string, object?> Attributes => throw new NotImplementedException();
        
        public OllamaGenerationService(string uri = "http://localhost:11434", string modelName = "llama3")
        {
            ModelUrl = uri;
            ModelName = modelName;
        }

        public IAsyncEnumerable<StreamingTextContent> GetStreamingTextContentsAsync(string prompt, PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<TextContent>> GetTextContentsAsync(string prompt, PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, CancellationToken cancellationToken = default)
        {
            var ollama = new OllamaApiClient(ModelUrl, ModelName);

            var completionResponse = await ollama.GetCompletion(prompt, null, CancellationToken.None);

            TextContent stc = new TextContent(completionResponse.Response);
            return new List<TextContent> { stc };
        }
    }
}