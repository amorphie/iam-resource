using System;
using BBT.Prism;
using BBT.Prism.EntityFrameworkCore.Sqlite;
using BBT.Prism.Modularity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace BBT.Resource.EntityFrameworkCore;

[Modules(
    typeof(ResourceApplicationTestModule),
    typeof(ResourceEntityFrameworkCoreModule),
    typeof(PrismEntityFrameworkCoreSqliteModule)
    )]
public class ResourceEntityFrameworkCoreTestModule : PrismModule
{
    private SqliteConnection? _sqliteConnection;

    public override void ConfigureServices(ModuleConfigurationContext context)
    {
        ConfigureInMemorySqlite(context.Services);
    }

    private void ConfigureInMemorySqlite(IServiceCollection services)
    {
        _sqliteConnection = CreateDatabaseAndGetConnection(services);
        
        services.AddPrismDbContext<ResourceDbContext>(options =>
        {
            options.UseSqlite(_sqliteConnection);
        });
    }

    public override void OnApplicationShutdown(ApplicationShutdownContext context)
    {
        _sqliteConnection?.Dispose();
    }

    private static SqliteConnection CreateDatabaseAndGetConnection(IServiceCollection services)
    {
        var connection = new PrismUnitTestSqliteConnection("Data Source=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<ResourceDbContext>()
            .UseSqlite(connection)
            .Options;

        var prodiver = services.GetServiceProviderOrNull();
        using var context = new ResourceDbContext(prodiver ?? throw new InvalidOperationException(), options);
        context.GetService<IRelationalDatabaseCreator>().CreateTables();

        return connection;
    }
}
