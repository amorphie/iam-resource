using System.IO;
using BBT.Prism.AspNetCore.Serilog;
using Microsoft.Extensions.Configuration;

namespace BBT.Resource;

public static class SerilogConfigurationHelper
{
    public static void Configure(string applicationName)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();

        var debugMode = false;
#if DEBUG
        debugMode = true;
#endif
        var builder = new SerilogConfigurationBuilder(applicationName, configuration)
            .AddDefaultConfiguration(debugMode);

        // Enable Open Telemetry
        builder.AddOpenTelemetry();

        // Example of adding a custom enricher
        // builder.AddEnrichment(new CustomEnricher());

        builder.Build();
    }
}
