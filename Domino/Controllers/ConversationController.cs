using Domino.Orchestrators;
using Microsoft.AspNetCore.Mvc;

namespace Domino.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConversationController : ControllerBase
    {
        private readonly ConversationOrchestrator _orchestrator;

        public ConversationController(ConversationOrchestrator orchestrator)
        {
            _orchestrator = orchestrator;
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartConversation()
        {
            var result = await _orchestrator.RunConversationAsync();
            return Ok(new { message = result });
        }
    }
}
