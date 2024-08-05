using System.IO;
using BBT.Prism.DependencyInjection;
using BBT.Prism.Timing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BBT.Resource.EntityFrameworkCore;

public class ResourceDbContextFactory: IDesignTimeDbContextFactory<ResourceDbContext>
{
    public ResourceDbContext CreateDbContext(string[] args)
    {
        // Only design context
        var services = new ServiceCollection();
        services.AddOptions();
        services.AddTransient<ILazyServiceProvider, LazyServiceProvider>();
        services.AddTransient<IClock, Clock>();
        var serviceProvider = services.BuildServiceProvider();
        
        var builder = new DbContextOptionsBuilder<ResourceDbContext>()
            .UseNpgsql(GetConnectionStringFromConfiguration(), b =>
            {
                b.MigrationsHistoryTable("__Resource_Migrations");
            });
        
        return new ResourceDbContext(serviceProvider, builder.Options);
    }
    
    private static string? GetConnectionStringFromConfiguration()
    {
        return BuildConfiguration()
            .GetConnectionString("Default");
    }
    
    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(
                Path.Combine(
                    Directory.GetCurrentDirectory(),
                    $"..{Path.DirectorySeparatorChar}BBT.Resource.HttpApi.Host"
                )
            )
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: false);

        return builder.Build();
    }
}