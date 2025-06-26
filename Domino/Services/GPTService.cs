using Domino.Models;
using Domino.Services.Interfaces;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Domino.Services
{
    public class GPTService : IGPTService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _model;
        private readonly ILogger<GPTService> _logger;

        public GPTService(HttpClient httpClient, IConfiguration configuration, ILogger<GPTService> logger)
        {
            _httpClient = httpClient;
            _apiKey = configuration["OpenAI:ApiKey"];
            _model = configuration["OpenAI:Model"] ?? "gpt-3.5-turbo";
            _logger = logger;

            if (string.IsNullOrWhiteSpace(_apiKey))
                throw new InvalidOperationException("Missing OpenAI API key in configuration.");

            _httpClient.BaseAddress = new Uri("https://api.openai.com/");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            _logger = logger;
        }

        public async Task<string> GetResponseAsync(string prompt, CancellationToken cancellationToken = default)
        {
            var request = new
            {
                model = _model,
                messages = new[] { new { role = "user", content = prompt } },
                temperature = 0.7
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync("v1/chat/completions", request, cancellationToken);

                if (response.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    _logger.LogWarning("Rate limit hit. OpenAI returned 429. Using fallback.");
                    return GetFallbackResponse();
                }

                response.EnsureSuccessStatusCode();

                using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);
                using var doc = await JsonDocument.ParseAsync(contentStream, cancellationToken: cancellationToken);

                return doc.RootElement
                          .GetProperty("choices")[0]
                          .GetProperty("message")
                          .GetProperty("content")
                          .GetString() ?? string.Empty;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling OpenAI. Returning fallback.");
                return GetFallbackResponse();
            }
        }

        private string GetFallbackResponse()
        {
            return "GPT is busy. Please try again later.";
        }
    }
}
