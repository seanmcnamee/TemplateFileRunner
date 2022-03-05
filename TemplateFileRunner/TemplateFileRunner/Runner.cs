using Microsoft.Extensions.Logging;
using TemplateFileRunner.Configuration;

namespace TemplateFileRunner.TemplateFileRunner
{
    public interface IRunner
    {
        Task RunApplication();
    }

    public class Runner : IRunner
    {
        private readonly IFileManipulation _fileManipulation;
        private readonly IFileTemplateSettingsValidator _validator;
        private readonly IFileTemplateSettings _settings;
        private readonly ILogger<Runner> _logger;
        public Runner(IFileManipulation fileManipulation, IFileTemplateSettingsValidator validator, IFileTemplateSettings settings, ILogger<Runner> logger)
        {
            _fileManipulation = fileManipulation;
            _validator = validator;
            _settings = settings;
            _logger = logger;
        }

        public async Task RunApplication()
        {
            if (!_validator.Validate())
                return;

            var templateFile = $"{_settings.ResourceDirectory}/{_settings.TemplateName}";
            var templateFileText = await _fileManipulation.GetFileContents(templateFile);

            var index = 0;
            foreach(var outputFile in _settings.OutputFiles)
            {
                var outputFileName = $"{outputFile.Prefix}{_settings.OutputBaseName}{outputFile.Suffix}{(outputFile.AppendCounterSuffix ? index : string.Empty)}{(outputFile.AppendDateTimeSuffix ? DateTime.Now.ToString("yyyyMMdd-HHmmss-fffffff") : string.Empty)}";
                var outputFileFullPath = $"{_settings.ResourceDirectory}/{_settings.OutputDirectory}/{outputFileName}.{_settings.OutputFileExtension}";

                var properties = outputFile.ReplacementProperties.GetPropertyValues(outputFile.ReplacementProperties);

                var outputFileContent = string.Format(templateFileText, properties);

                var success = await _fileManipulation.WriteToFile(outputFileFullPath, outputFileContent);
                if (success)
                    _logger.LogInformation("Created file {outputFileFullPath} successfully", outputFileFullPath);
                else
                    _logger.LogWarning("Unable to write to file {outputFileFullPath}", outputFileFullPath);

                index++;
            }

        }
    }
}
