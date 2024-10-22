namespace SemanticKernelTest.Services.Scrappers;

public interface IDirectoryScrapper
{
    /// <summary>
    /// Recursively exctracts all files from provided directory
    /// </summary>
    /// <returns>Dictionary of filepath as a key and filecontent as a value</returns>
    Dictionary<string, string> GetSourceFilesFromDirectory(string rootDirectory);
}