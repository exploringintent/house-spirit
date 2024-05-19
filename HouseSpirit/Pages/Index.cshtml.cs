using HouseSpirit.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HouseSpirit.Pages;

public class IndexModel : PageModel
{

    private readonly ILogger<IndexModel> _logger;
    private readonly IGenerationService _generationService;

    public IndexModel(ILogger<IndexModel> logger, IGenerationService generationService)
    {
        _logger = logger;
        _generationService = generationService;
        this.Message = string.Empty;
        this.Conversation = string.Empty;
    }

    [BindProperty]
    public string Message { get; set; }

    public string Conversation { get; set; }

    public IActionResult OnPostSendChat()
    {
        var conversation = HttpContext.Session.GetString("conversation");
        conversation += string.Format("{0}{1}", Message, Environment.NewLine);
        Message = _generationService.GiveChatResponse(Message);
        conversation += string.Format("{0}{1}", Message, Environment.NewLine);
        HttpContext.Session.SetString("conversation", conversation);
        Conversation = conversation;
        return Page();
    }

    public IActionResult OnPostClearChat()
    {
        HttpContext.Session.SetString("conversation", string.Empty);
        Conversation = string.Empty;
        return Page();
    }
}
