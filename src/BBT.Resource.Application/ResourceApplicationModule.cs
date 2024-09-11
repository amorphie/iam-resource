using BBT.Prism.Application;
using BBT.Prism.AutoMapper;
using BBT.Prism.Mapper;
using BBT.Prism.Modularity;
using BBT.Resource.Policies;
using BBT.Resource.Privileges;
using BBT.Resource.Resources;
using BBT.Resource.Resources.Authorize;
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

        var authorizeConfig = context.Services
            .GetConfiguration()
            .GetSection(CheckAuthorizeOptions.Name);
        context.Services.Configure<CheckAuthorizeOptions>(authorizeConfig);

        context.Services.AddTransient<MultiLingualRoleDefinitionObjectMapper>();
        context.Services.AddTransient<MultiLingualResourceGroupObjectMapper>();
        context.Services.AddTransient<MultiLingualRoleGroupObjectMapper>();
        context.Services.AddTransient<MultiLingualRoleObjectMapper>();
        context.Services.AddTransient<MultiLingualResourceObjectMapper>();

        context.Services.AddTransient<IResourceGroupAppService, ResourceGroupAppService>();
        context.Services.AddTransient<IResourceAppService, ResourceAppService>();
        context.Services.AddTransient<IRuleAppService, RuleAppService>();
        context.Services.AddTransient<IPrivilegeAppService, PrivilegeAppService>();
        context.Services.AddTransient<IRoleDefinitionAppService, RoleDefinitionAppService>();
        context.Services.AddTransient<IRoleGroupAppService, RoleGroupAppService>();
        context.Services.AddTransient<IRoleAppService, RoleAppService>();
        context.Services.AddTransient<IResourceAuthorizeAppService, ResourceAuthorizeAppService>();
        context.Services.AddTransient<IResourceAuthorizeAppService, ResourceAuthorizeAppService>();
        context.Services.AddTransient<IPolicyAppService, PoliciesAppService>();

        context.Services.AddTransient<CheckAuthorizeByRule>();
        context.Services.AddTransient<CheckAuthorizeByPrivilege>();
        context.Services.AddSingleton<ICheckAuthorizeFactory, CheckAuthorizeFactory>();
    }
}
