using Microsoft.Extensions.Logging;

namespace TemplateFileRunner
{
    public interface IFileTemplateSettingsValidator
    {
        bool Validate();
    }

    public class FileTemplateSettingsValidator : IFileTemplateSettingsValidator
    {
        private readonly IFileTemplateSettings _fileTemplateSettings;
        private readonly ILogger<FileTemplateSettingsValidator> _logger;
        public FileTemplateSettingsValidator(IFileTemplateSettings fileTemplateSettings, ILogger<FileTemplateSettingsValidator> logger)
        {
            _fileTemplateSettings = fileTemplateSettings;
            _logger = logger;
        }

        public bool Validate()
        {
            var errorList = new List<string>();
            const string errorTemplate = "Missing from Configuration: {0}";

            if (_fileTemplateSettings == null)
            {
                _logger.LogError("Configuration is null");
                return false;
            }
            
            if (string.IsNullOrWhiteSpace(_fileTemplateSettings.ResourceDirectory))
                errorList.Add(string.Format(errorTemplate, nameof(_fileTemplateSettings.ResourceDirectory)));

            if (string.IsNullOrWhiteSpace(_fileTemplateSettings.TemplateName))
                errorList.Add(string.Format(errorTemplate, nameof(_fileTemplateSettings.TemplateName)));

            if (string.IsNullOrWhiteSpace(_fileTemplateSettings.OutputDirectory))
                errorList.Add(string.Format(errorTemplate, nameof(_fileTemplateSettings.OutputDirectory)));

            if (string.IsNullOrWhiteSpace(_fileTemplateSettings.OutputBaseName))
                errorList.Add(string.Format(errorTemplate, nameof(_fileTemplateSettings.OutputBaseName)));

            if (string.IsNullOrWhiteSpace(_fileTemplateSettings.OutputFileExtension))
                errorList.Add(string.Format(errorTemplate, nameof(_fileTemplateSettings.OutputFileExtension)));

            if (_fileTemplateSettings.OutputFiles?.Any() != true)
            {
                errorList.Add($"{nameof(_fileTemplateSettings.OutputFiles)} must have at least one record");
            } else
            {
                var possibleDuplicateFileNames = new List<string>();
                foreach (var outputFile in _fileTemplateSettings.OutputFiles)
                {
                    if (!outputFile.AppendCounterSuffix && !outputFile.AppendDateTimeSuffix)
                    {
                        var fileName = $"{outputFile.Prefix}{outputFile.Suffix}";
                        if (possibleDuplicateFileNames.Contains(fileName))
                        {
                            errorList.Add($"Output file name \"{fileName}\" must be unique");
                        }
                        possibleDuplicateFileNames.Add(fileName);
                    }
                    if (outputFile.ReplacementProperties?.HasNullValue(outputFile.ReplacementProperties) != false)
                    {
                        errorList.Add($"Not all replacement values are specified for output file {_fileTemplateSettings.OutputFiles.ToList().IndexOf(outputFile)}");
                    }
                }
            }

            if (errorList.Any())
                _logger.LogError("The following errors exist for the file template settings configuration: \n\t{errors}", string.Join("\n\t", errorList));

            return !errorList.Any();
        }
    }
}