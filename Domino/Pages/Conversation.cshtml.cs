using Domino.Orchestrators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Domino.Pages
{
    public class ConversationModel : PageModel
    {
        private readonly ConversationOrchestrator _orchestrator;

        public string Result { get; private set; }

        public ConversationModel(ConversationOrchestrator orchestrator)
        {
            _orchestrator = orchestrator;
        }

        public async Task OnPostAsync()
        {
            Console.WriteLine("Starting conversation orchestrator...");
            Result = await _orchestrator.RunConversationAsync();
        }
    }
}
