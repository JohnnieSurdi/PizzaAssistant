using Domino.Services.Interfaces;
using Microsoft.CognitiveServices.Speech;

namespace Domino.Services
{
    public class TextToSpeechService : ITextToSpeechService
    {
        private readonly string _subscriptionKey;
        private readonly string _region;

        public TextToSpeechService(IConfiguration config)
        {
            _subscriptionKey = config["AzureSpeech:SubscriptionKey"];
            _region = config["AzureSpeech:Region"];

            if (string.IsNullOrWhiteSpace(_subscriptionKey) || string.IsNullOrWhiteSpace(_region))
                throw new InvalidOperationException("Missing Azure Speech configuration.");
        }

        public async Task<string> SpeakTextAsync(string text)
        {
            var speechConfig = SpeechConfig.FromSubscription(_subscriptionKey, _region);
            speechConfig.SpeechSynthesisVoiceName = "en-US-AvaMultilingualNeural";

            using var synthesizer = new SpeechSynthesizer(speechConfig);

            var result = await synthesizer.SpeakTextAsync(text);

            switch (result.Reason)
            {
                case ResultReason.SynthesizingAudioCompleted:
                    return "Speech synthesized successfully.";
                case ResultReason.Canceled:
                    var cancellation = SpeechSynthesisCancellationDetails.FromResult(result);
                    return $"Canceled: {cancellation.Reason}, {cancellation.ErrorDetails}";
                default:
                    return "Unknown speech synthesis result.";
            }
        }
    }
}
