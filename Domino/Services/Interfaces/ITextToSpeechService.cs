namespace Domino.Services.Interfaces
{
    public interface ITextToSpeechService
    {
        Task<string> SpeakTextAsync(string text);
    }
}
