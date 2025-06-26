namespace Domino.Services.Interfaces
{
    public interface IOllamaService
    {
        Task<string> GetResponseAsync(string prompt, CancellationToken cancellationToken = default);
    }
}
