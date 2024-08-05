using BBT.Prism;
using BBT.Prism.Data.Seeding;
using BBT.Prism.Modularity;
using BBT.Prism.Threading;
using Microsoft.Extensions.DependencyInjection;

namespace BBT.Resource;

[Modules(
    typeof(PrismTestBaseModule),
    typeof(PrismDataSeedingModule)
    )]
public class ResourceTestBaseModule: PrismModule
{
    public override void ConfigureServices(ModuleConfigurationContext context)
    {
        context.Services.AddTransient<ResourceTestDataSeedContributor>();
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
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
}