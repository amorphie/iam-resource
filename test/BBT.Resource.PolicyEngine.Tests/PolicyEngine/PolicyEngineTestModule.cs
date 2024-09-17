using BBT.Prism.Modularity;
using BBT.Resource.PolicyEngine.Evaluations;
using Microsoft.Extensions.DependencyInjection;

namespace BBT.Resource.PolicyEngine;

[Modules(
    typeof(PolicyEngineModule),
    typeof(ResourceTestBaseModule)
)]
public class PolicyEngineTestModule : PrismModule
{
    public override void ConfigureServices(ModuleConfigurationContext context)
    {
        // Mock Service
        context.Services.AddTransient<IPermissionStore, PermissionMockService>();
    }
}
