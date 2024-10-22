using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using SemanticKernelTest.Services.Rag;
using SemanticKernelTest.Services.Scrappers;
using SemanticKernelTest.Services.UserInterface.Chats;

namespace SemanticKernelTest;

/// <inheritdoc cref="ICore"/>
public sealed class Core: ICore
{
    private readonly IDirectoryScrapper _directoryScrapper;
    private readonly IConfiguration _configuration;
    private readonly IChatsService _chatsService;
    private readonly IRagImporter _ragImporter;
    
    //TODO: remove shortcut
    public const string MemoryCollectionName = "IoCoreSources";

    /// <summary>
    /// Ctor.
    /// </summary>
    /// <param name="directoryScrapper"></param>
    /// <param name="configuration"></param>
    /// <param name="ragImporter"></param>
    /// <param name="chatsService"></param>
    public Core(
        IDirectoryScrapper directoryScrapper,
        IConfiguration configuration, IRagImporter ragImporter, IChatsService chatsService)
    {
        _directoryScrapper = directoryScrapper;
        _configuration = configuration;
        _chatsService = chatsService;
        _ragImporter = ragImporter;
    }

    ///<inheritdoc cref="ICore"/>
    [Experimental("SKEXP0050")]
    public async Task RunAsync(CancellationToken cancellationToken)
    {
       var locations = _configuration.GetSection("Scrapper:Locations")
            .Get<List<string>>();

        Dictionary<string, string> scrappedFiles = _directoryScrapper.GetSourceFilesFromDirectory(locations[0]);
    
        await _ragImporter.ImportMemoryCollection(MemoryCollectionName, scrappedFiles, cancellationToken);
        
        await _chatsService.UseChats(cancellationToken);
    }
}