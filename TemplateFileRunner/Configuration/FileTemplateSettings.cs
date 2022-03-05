namespace TemplateFileRunner.Configuration
{
    public interface IFileTemplateSettings
    {
        string ResourceDirectory { get; set; }
        string TemplateName { get; set; }
        string OutputDirectory { get; set; }
        string OutputBaseName { get; set; }
        string OutputFileExtension { get; set; }
        FileOutputProperties[] OutputFiles { get; set; }
    }


    public class FileTemplateSettings : IFileTemplateSettings
    {
        public string ResourceDirectory { get; set; }
        public string TemplateName { get; set; }
        public string OutputDirectory { get; set; }
        public string OutputBaseName { get; set; }
        public string OutputFileExtension { get; set; }
        public FileOutputProperties[] OutputFiles { get; set; }
    }

    public class FileOutputProperties
    {
        //Disable all options by default
        public FileOutputProperties()
        {
            Prefix = string.Empty;
            Suffix = string.Empty;
            AppendDateTimeSuffix = false;
            AppendCounterSuffix = false;
            ReplacementProperties = new ReplacementProperties();
        }

        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public bool AppendDateTimeSuffix { get; set; }
        public bool AppendCounterSuffix { get; set; }
        public ReplacementProperties ReplacementProperties { get; set; }
    }

    public class ReplacementProperties
    {
        public string Value0 { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string Value3 { get; set; }

        public bool HasNullValue(ReplacementProperties obj) => 
            typeof(ReplacementProperties).GetProperties()
            .Select(prop => string.IsNullOrWhiteSpace(prop.GetValue(obj, null)?.ToString()))
            .Where(isNull => isNull)
            .Any();

        public string?[] GetPropertyValues(ReplacementProperties obj) =>
            typeof(ReplacementProperties).GetProperties()
            .Select(prop => prop.GetValue(obj, null)?.ToString())
            .Where(value => value != null)
            .ToArray();
    }
}