namespace HouseSpirit.Services
{
    public interface IGenerationService
    {
        string GiveChatResponse(string userPrompt);
        string GiveCompletionResponse(string userPrompt);
    }
}