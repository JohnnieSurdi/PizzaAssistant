namespace Domino.Services.Interfaces
{
    public interface IGPTService
    {
        Task<string> GetResponseAsync(string prompt, CancellationToken cancellationToken = default);
    }
}
