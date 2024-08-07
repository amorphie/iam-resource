using System;
using BBT.Prism.Domain.EntityFrameworkCore;
using BBT.Resource.Data;
using BBT.Prism.EntityFrameworkCore;
using BBT.Prism.Modularity;
using BBT.Resource.Privileges;
using BBT.Resource.Resources;
using BBT.Resource.Roles;
using BBT.Resource.Rules;
using Microsoft.Extensions.DependencyInjection;

namespace BBT.Resource.EntityFrameworkCore;

[Modules(
    typeof(ResourceDomainModule),
    typeof(PrismEntityFrameworkCoreModule)
)]
public class ResourceEntityFrameworkCoreModule : PrismModule
{
    public override void PreConfigureServices(ModuleConfigurationContext context)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    public override void ConfigureServices(ModuleConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        context.Services.AddTransient<IResourceDbSchemaMigrator, ResourceDbSchemaMigrator>();

        /* App repositories */

        context.Services.AddTransient<IResourceGroupRepository, EfCoreResourceGroupRepository>();
        context.Services.AddTransient<IResourceRepository, EfCoreResourceRepository>();
        context.Services.AddTransient<IRuleRepository, EfCoreRuleRepository>();
        context.Services.AddTransient<IPrivilegeRepository, EfCorePrivilegeRepository>();
        context.Services.AddTransient<IRoleDefinitionRepository, EfCoreRoleDefinitionRepository>();
        context.Services.AddTransient<IRoleGroupRepository, EfCoreRoleGroupRepository>();
        context.Services.AddTransient<IRoleRepository, EfCoreRoleRepository>();
    }
}
