using BBT.Prism.Application;
using BBT.Prism.AutoMapper;
using BBT.Prism.Modularity;
using BBT.Resource.Privileges;
using BBT.Resource.Resources;
using BBT.Resource.Roles;
using BBT.Resource.Rules;
using Microsoft.Extensions.DependencyInjection;

namespace BBT.Resource;

[Modules(
    typeof(ResourceDomainModule),
    typeof(ResourceApplicationContractsModule),
    typeof(PrismDddApplicationModule),
    typeof(PrismAutoMapperModule)
)]
public class ResourceApplicationModule : PrismModule
{
    public override void ConfigureServices(ModuleConfigurationContext context)
    {
        Configure<PrismAutoMapperOptions>(options => { options.AddMaps<ResourceApplicationModule>(validate: true); });

        context.Services.AddTransient<IResourceGroupAppService, ResourceGroupAppService>();
        context.Services.AddTransient<IResourceAppService, ResourceAppService>();
        context.Services.AddTransient<IRuleAppService, RuleAppService>();
        context.Services.AddTransient<IPrivilegeAppService, PrivilegeAppService>();
        context.Services.AddTransient<IRoleDefinitionAppService, RoleDefinitionAppService>();
    }
}
