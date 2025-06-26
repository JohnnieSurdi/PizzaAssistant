using Domino.Services.Interfaces;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Domino.Services
{
    public class SpeechToTextService : ISpeechToTextService
    {
        private readonly string _subscriptionKey;
        private readonly string _region;

        public SpeechToTextService(IConfiguration configuration)
        {
            _subscriptionKey = configuration["AzureSpeech:SubscriptionKey"];
            _region = configuration["AzureSpeech:Region"];

            if (string.IsNullOrWhiteSpace(_subscriptionKey) || string.IsNullOrWhiteSpace(_region))
            {
                throw new InvalidOperationException("Azure Speech configuration is missing or invalid.");
            }
        }

        public async Task<string> RecognizeSpeechAsync()
        {
            var config = SpeechConfig.FromSubscription(_subscriptionKey, _region);

            using var recognizer = new SpeechRecognizer(config);

            Console.WriteLine("Speak into your microphone...");

            var result = await recognizer.RecognizeOnceAsync();

            return result.Reason switch
            {
                ResultReason.RecognizedSpeech => result.Text,
                ResultReason.NoMatch => "No speech could be recognized.",
                ResultReason.Canceled => $"Recognition canceled: {CancellationDetails.FromResult(result).ErrorDetails}",
                _ => "Unknown recognition result."
            };
        }
    }
}
