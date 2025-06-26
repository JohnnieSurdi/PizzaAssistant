namespace Domino.Services.Interfaces
{
    public interface ISpeechToTextService
    {
        Task<string> RecognizeSpeechAsync();
    }
}
