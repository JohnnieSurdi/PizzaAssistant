using Domino.Models;
using Domino.Services.Interfaces;

namespace Domino.Services
{
    public class OllamaService : IOllamaService
    {
        private readonly HttpClient _httpClient;
        private readonly string _model;

        public OllamaService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _model = configuration["Ollama:Model"] ?? "phi3:mini";
        }

        public async Task<string> GetResponseAsync(string prompt, CancellationToken cancellationToken = default)
        {
            var request = new
            {
                model = _model,
                prompt = prompt,
                stream = false,
                temperature = 0.7
            };

            var response = await _httpClient.PostAsJsonAsync("/api/generate", request, cancellationToken);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<GenerationResponse>(cancellationToken: cancellationToken);
            return result?.Response ?? "⚠️ No response from Ollama model.";
        }
    }
}
