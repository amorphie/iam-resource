using BBT.Prism;
using BBT.Prism.Data.Seeding;
using BBT.Prism.EntityFrameworkCore.Sqlite;
using BBT.Prism.Modularity;
using BBT.Prism.Threading;
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
        context.Services.AddTransient<ResourceTestDataSeedContributor>();
        ConfigureInMemorySqlite(context.Services);
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        using var scope = context.ServiceProvider.CreateScope();
        scope.ServiceProvider
            .GetRequiredService<ResourceDbContext>()
            .GetService<IRelationalDatabaseCreator>().CreateTables();

        SeedTestData(context);
    }
    
    private static void SeedTestData(ApplicationInitializationContext context)
    {
        AsyncHelper.RunSync(async () =>
        {
            using var scope = context.ServiceProvider.CreateScope();
            await scope.ServiceProvider
                .GetRequiredService<IDataSeeder>()
                .SeedAsync(new DataSeedContext());
        });
    }
    
    private void ConfigureInMemorySqlite(IServiceCollection services)
    {
        _sqliteConnection = CreateDatabaseAndGetConnection(services);

        services.AddPrismDbContext<ResourceDbContext>(options => { options.UseSqlite(_sqliteConnection); });
    }

    public override void OnApplicationShutdown(ApplicationShutdownContext context)
    {
        _sqliteConnection?.Dispose();
    }

    private static SqliteConnection CreateDatabaseAndGetConnection(IServiceCollection services)
    {
        var connection = new PrismUnitTestSqliteConnection("Data Source=:memory:");
        connection.Open();
        return connection;
    }
}
