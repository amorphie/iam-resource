using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Asp.Versioning;
using BBT.Resource.Extensions;
using Dapr.Client;
using Dapr.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace BBT.Resource;

internal class Program
{
    private const string ApplicationName = "Resource";

    public async static Task<int> Main(string[] args)
    {
        SerilogConfigurationHelper.Configure(ApplicationName!);

        try
        {
            var daprClient = new DaprClientBuilder()
                .Build();
            
            using (var tokenSource = new CancellationTokenSource(20000))
            {
                try
                {
                    await daprClient.WaitForSidecarAsync(tokenSource.Token);
                }
                catch (Exception ex)
                {
                    Log.Fatal(ex, $"{ApplicationName} Dapr Sidecar doesn't respond!", ex.ToString());
                    return 1;
                }
            }

            Log.Information($"Starting {ApplicationName}.");
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());
            builder.Configuration.AddDaprSecretStore("resource-secretstore", daprClient);
            builder.WebHost.ConfigureKestrel(option => option.AddServerHeader = false);
            builder.ConfigureOpenTelemetryLogging();
            builder.Host.UseSerilog();
            await builder.AddApplicationAsync<ResourceHttpApiHostModule>(options =>
            {
                options.ApplicationName = ApplicationName;
            });

            var app = builder.Build();
            app.MapControllers();
            var versionSet = app
                .NewApiVersionSet()
                .HasApiVersion(new ApiVersion(1))
                .HasApiVersion(new ApiVersion(2))
                .ReportApiVersions()
                .Build();

            var versionedGroup = app
                .MapGroup("api/v{version:apiVersion}")
                .WithApiVersionSet(versionSet);

            app.MapEndpoints(versionedGroup);
            await app.InitializeApplicationAsync();
            await app.RunAsync();
            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, $"{ApplicationName} terminated unexpectedly!");
            return 1;
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }
    }
}