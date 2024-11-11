using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using SemanticKernelTest.Services.Speech;
using SemanticKernelTest.Services.Speech.Entities;

namespace SemanticKernelTest.Services.UserInterface.Chats.Chat;

public class ChatTts: IChat
{
    private readonly ITtsService _ttsService;  
    private readonly Kernel _kernel;
    private readonly string _modelName; 
    public ChatTts(Kernel kernel, IConfiguration configuration, ITtsService ttsService)
    {
        _ttsService = ttsService;
        _kernel = kernel;
        _modelName = configuration.GetValue<string>("Models:TextModel");
    }
    public async Task SendMessage(string message, CancellationToken cancellationToken)
    {
        const string prompt = @"
                        Question: {{$input}}
                        Answer the question using the memory content: {{Recall}}";
            
        OpenAIPromptExecutionSettings settings = new()
        {
            ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
            Temperature = 0.7f
        };

        var arguments = new KernelArguments(settings)
        {
            {
                "input", message
            },
            {
                "collection", Core.MemoryCollectionName
            }
        };


        Console.WriteLine(
            $"{_modelName} response (using semantic memory).");

        var response = _kernel.InvokePromptStreamingAsync(prompt,
            arguments, cancellationToken: cancellationToken);
        var sb = new StringBuilder();
        await foreach (var result in response)
        {
            var s = (StreamingChatMessageContent)result;
            sb.Append(s.Content);
            Console.Write(result);
        }

        var request = new SpeechRequest( 
            sb.ToString()
            );
        
        await _ttsService.GenerateAndPlaySpeechAsync(request);
    }
}