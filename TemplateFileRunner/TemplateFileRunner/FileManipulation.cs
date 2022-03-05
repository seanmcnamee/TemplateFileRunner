
using Microsoft.Extensions.Logging;

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
        private readonly IFileTemplateSettings _settings;
        private readonly ILogger<FileManipulation> _logger;
        public FileManipulation(IFileTemplateSettings settings, ILogger<FileManipulation> logger)
        {
            _settings = settings;
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
            _logger.LogInformation("Path: {path}", Path.GetFullPath($"../../../{outputFileName}"));

            try
            {
                using (StreamWriter outputFile = new StreamWriter($"../../../{outputFileName}"))
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