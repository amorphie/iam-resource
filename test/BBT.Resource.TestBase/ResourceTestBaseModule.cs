using BBT.Prism;
using BBT.Prism.Data.Seeding;
using BBT.Prism.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace BBT.Resource;

[Modules(
    typeof(PrismTestBaseModule),
    typeof(PrismDataSeedingModule)
)]
public class ResourceTestBaseModule : PrismModule
{
    public override void ConfigureServices(ModuleConfigurationContext context)
    {
        context.Services.AddSingleton<TestData>();
    }
}
