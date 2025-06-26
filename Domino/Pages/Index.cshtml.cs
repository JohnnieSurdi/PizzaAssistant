using Domino.Services;
using Domino.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Domino.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ISpeechToTextService _speechService;
        private readonly ITextToSpeechService _ttsService;

        public string? RecognizedText { get; private set; }

        [BindProperty]
        public string? InputText { get; set; }

        public string? TtsMessage { get; private set; }

        public IndexModel(ILogger<IndexModel> logger, ISpeechToTextService speechService, ITextToSpeechService ttsService)
        {
            _logger = logger;
            _speechService = speechService;
            _ttsService = ttsService;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostRecognizeAsync()
        {
            RecognizedText = await _speechService.RecognizeSpeechAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostSpeakAsync()
        {
            if (!string.IsNullOrWhiteSpace(InputText))
            {
                TtsMessage = await _ttsService.SpeakTextAsync(InputText);
            }
            return Page();
        }

    }
}
