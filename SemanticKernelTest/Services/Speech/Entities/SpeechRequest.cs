using System.Text.Json.Serialization;

namespace SemanticKernelTest.Services.Speech.Entities;

public class SpeechRequest
{
        [JsonPropertyName("model")]
        public string Model { get; set; } = "tts-1";

        [JsonPropertyName("input")]
        public string Input { get; set; }

        [JsonPropertyName("voice")]
        public string Voice { get; set; }

        [JsonPropertyName("response_format")]
        public string ResponseFormat { get; set; }

        [JsonPropertyName("speed")]
        public double Speed { get; set; }

        public SpeechRequest(string input, string voice = "alloy", string responseFormat = "mp3", double speed = 1.0)
        {
            Input = input;
            Voice = voice;
            ResponseFormat = responseFormat;
            Speed = speed;
        }
}