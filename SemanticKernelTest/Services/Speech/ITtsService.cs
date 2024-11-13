using SemanticKernelTest.Services.Speech.Entities;

namespace SemanticKernelTest.Services.Speech;

public interface ITtsService
{
    Task GenerateAndPlaySpeechAsync(SpeechRequest request);
    Task StopPlaybackAsync();
}