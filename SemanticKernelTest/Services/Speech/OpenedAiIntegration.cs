using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using NetCoreAudio;
using SemanticKernelTest.Services.Speech.Entities;

namespace SemanticKernelTest.Services.Speech;

public class OpenedAiIntegration: ITtsService
{
    private static readonly HttpClient HttpClient = new HttpClient();
    private readonly IConfiguration _configuration;
    private readonly Player _player;

    public OpenedAiIntegration(IConfiguration configuration)
    {
        _configuration = configuration;
        _player = new Player();
    }
    public async Task GenerateAndPlaySpeechAsync(SpeechRequest request)
    {
        var jsonContent = JsonSerializer.Serialize(request);

        using var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        try
        {
            HttpResponseMessage response = await HttpClient.PostAsync(_configuration.GetValue<string>("TtsEndpoint"), content);
            response.EnsureSuccessStatusCode();

            await using Stream stream = await response.Content.ReadAsStreamAsync();
            await using var fileStream = new FileStream("speech.mp3", FileMode.Create, FileAccess.Write, FileShare.None);
            await stream.CopyToAsync(fileStream);

            Console.WriteLine("\n Speech playback started.");
            
            
            await _player.Play("speech.mp3");

            Console.WriteLine("\n Speech playback finished.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public async Task StopPlaybackAsync()
    {
        await _player.Stop();
    }
}