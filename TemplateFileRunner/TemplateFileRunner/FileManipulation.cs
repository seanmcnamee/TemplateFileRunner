
using Microsoft.Extensions.Logging;
using TemplateFileRunner.Configuration;

namespace TemplateFileRunner.TemplateFileRunner
{
    public interface IFileManipulation
    {
        public Task LogFileContents(string inputFileName);
        public Task<string> GetFileContents(string inputFileName);
        public Task<bool> WriteToFile(string outputFileName, string fileContents);
    }

    public class FileManipulation : IFileManipulation
    {
        private readonly IEnvironmentSettings _environmentSettings;
        private readonly ILogger<FileManipulation> _logger;
        public FileManipulation(IEnvironmentSettings environmentSettings, ILogger<FileManipulation> logger)
        {
            _environmentSettings = environmentSettings;
            _logger = logger;
        }

        public async Task LogFileContents(string inputFileName)
        {
            string fileText;
            try
            {
                using (var sr = new StreamReader(inputFileName))
                {
                    fileText = await sr.ReadToEndAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Unable to reading file {inputFileName}. Exception: {ex}", inputFileName, ex);
                fileText = ex.Message;
            }

            _logger.LogInformation(fileText);
        }


        public async Task<string> GetFileContents(string inputFileName)
        {
            try
            {
                using (var sr = new StreamReader(inputFileName))
                {
                    return await sr.ReadToEndAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Unable to reading file {inputFileName}. Exception: {ex}", inputFileName, ex);
                return string.Empty;
            }
        }

        public async Task<bool> WriteToFile(string outputFileName, string fileContents)
        {
            var outputPathLocationPrefix = _environmentSettings.ExecutedFromSolution ? "../../../" : string.Empty;
            _logger.LogInformation("Path: {path}", Path.GetFullPath($"{outputPathLocationPrefix}{outputFileName}"));

            try
            {
                using (StreamWriter outputFile = new StreamWriter($"{outputPathLocationPrefix}{outputFileName}"))
                {
                    await outputFile.WriteAsync(fileContents);
                }
                return true;
            }
            catch (Exception ex)
            {

                _logger.LogError("Unable to write to file {outputFileName}. Exception: {ex}", outputFileName, ex);
                return false;
            }
        }
    }
}