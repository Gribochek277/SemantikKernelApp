using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.Text.Json;
using NetCoreAudio;
using SemanticKernelTest.Services.Speech.Entities;

namespace SemanticKernelTest.Services.Speech;

public class OpenedAiIntegration: ITtsService
{
    private static readonly HttpClient httpClient = new HttpClient();
    private const string Url = "http://localhost:8000/v1/audio/speech";

    public async Task GenerateAndPlaySpeechAsync(SpeechRequest request)
    {
        var jsonContent = JsonSerializer.Serialize(request);

        using var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        try
        {
            // Выполнение запроса
            var response = await httpClient.PostAsync(Url, content);
            response.EnsureSuccessStatusCode();

            // Чтение содержимого ответа в память
            // Сохранение ответа в файл
            using var stream = await response.Content.ReadAsStreamAsync();
            using var fileStream = new FileStream("speech.mp3", FileMode.Create, FileAccess.Write, FileShare.None);
            await stream.CopyToAsync(fileStream);

            // Воспроизведение аудио через System.Media.SoundPlayer
            var player = new Player();
            await player.Play("speech.mp3"); // Проигрывает аудио и ожидает окончания

            Console.WriteLine("end of speech");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}