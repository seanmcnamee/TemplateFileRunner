namespace TemplateFileRunner.Configuration
{
    public interface IEnvironmentSettings
    {
        bool ExecutedFromSolution { get; set; }
    }
    public class EnvironmentSettings : IEnvironmentSettings
    {
        public bool ExecutedFromSolution { get; set; }
    }
}
