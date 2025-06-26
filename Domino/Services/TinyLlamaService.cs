using Domino.Models;
using Domino.Services.Interfaces;
using System.Text.Json;

namespace Domino.Services
{
    public class TinyLlamaService : IOllamaService
    {
        private readonly HttpClient _httpClient;
        private readonly string _model;

        private readonly List<ChatMessage> _chatHistory = new();

        public TinyLlamaService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _model = configuration["TinyLlama:Model"] ?? "tinyllama:chat";
        }

        public async Task<string> GetResponseAsync(string userInput, CancellationToken cancellationToken = default)
        {
            _chatHistory.Add(new ChatMessage("user", userInput));

            var request = new
            {
                model = _model,
                messages = _chatHistory.Select(m => new { role = m.Role, content = m.Content }).ToArray(),
                stream = false
            };

            var response = await _httpClient.PostAsJsonAsync("/api/chat", request, cancellationToken);
            response.EnsureSuccessStatusCode();

            using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);
            using var doc = await JsonDocument.ParseAsync(contentStream, cancellationToken: cancellationToken);

            var content = doc.RootElement
                             .GetProperty("message")
                             .GetProperty("content")
                             .GetString() ?? string.Empty;

            _chatHistory.Add(new ChatMessage("assistant", content));

            return content;
        }
    }
}
