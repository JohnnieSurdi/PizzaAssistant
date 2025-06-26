using Domino.Services.Interfaces;

namespace Domino.Orchestrators
{
    public class ConversationOrchestrator
    {
        private readonly ISpeechToTextService _speechService;
        private readonly IOllamaService _ollamaService;
        private readonly ITextToSpeechService _speechOutput;

        public ConversationOrchestrator(
            ISpeechToTextService speechService,
            IOllamaService ollamaService,
            ITextToSpeechService speechOutput)
        {
            _speechService = speechService;
            _ollamaService = ollamaService;
            _speechOutput = speechOutput;

        }

        public async Task<string> RunConversationAsync()
        {
            var userText = await _speechService.RecognizeSpeechAsync();
            Console.WriteLine($"User said: {userText}");
            Console.WriteLine("Processing user input...");
            var gptResponse = await _ollamaService.GetResponseAsync(userText);
            var speechResult = await _speechOutput.SpeakTextAsync(gptResponse);

            return $"You said: \"{userText}\" \nGPT replied: \"{gptResponse}\" \nTTS Status: {speechResult}";
        }
    }
}
