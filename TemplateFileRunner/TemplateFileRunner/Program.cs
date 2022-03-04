using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TemplateFileRunner;
using TemplateFileRunner.TemplateFileRunner;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(ConfigureServices)
    .ConfigureLogging(logging => logging.AddConsole())
    .Build();

//Run application
var runner = host.Services.GetService<IRunner>();
if (runner != null)
    await runner.RunApplication();


await host.RunAsync();


static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
{
    //Create IConfiguraion client
    services.AddSingleton<IConfiguration>(new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .AddEnvironmentVariables()
        .Build());

    //Get Settings from configuration
    services.AddSingleton<IFileTemplateSettings>((services) => 
        services.GetService<IConfiguration>().GetRequiredSection("FileTemplateSettings").Get<FileTemplateSettings>());

    services.AddSingleton<IFileTemplateSettingsValidator, FileTemplateSettingsValidator>();
    services.AddSingleton<IFileManipulation, FileManipulation>();
    services.AddSingleton<IRunner, Runner>();
}