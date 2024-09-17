using System;
using BBT.Resource.Data;
using BBT.Prism.Data.Seeding;
using BBT.Prism.Domain;
using BBT.Prism.Modularity;
using BBT.Prism.Timing;
using BBT.Resource.Permissions;
using BBT.Resource.Permissions.CheckProviders;
using BBT.Resource.Permissions.GrantProviders;
using BBT.Resource.Resources;
using Microsoft.Extensions.DependencyInjection;

namespace BBT.Resource;

[Modules(
    typeof(PrismDddDomainModule),
    typeof(ResourceDomainSharedModule),
    typeof(PrismDataSeedingModule)
)]
public class ResourceDomainModule : PrismModule
{
    public override void ConfigureServices(ModuleConfigurationContext context)
    {
        Configure<ClockOptions>(options => { options.Kind = DateTimeKind.Utc; });

        context.Services.AddTransient<ResourceDbMigrationService>();
        context.Services.AddTransient<ResourceDataSeedContributor>();
        context.Services.AddTransient<ResourceManager>();

        context.Services.AddTransient<UserPermissionCheckStrategy>();
        context.Services.AddTransient<RolePermissionCheckStrategy>();
        context.Services.AddTransient<ClientPermissionCheckStrategy>();
        context.Services.AddSingleton<PermissionCheckStrategyFactory>();
        
        context.Services.AddTransient<UserPermissionGrantStrategy>();
        context.Services.AddTransient<DefaultPermissionGrantStrategy>();
        context.Services.AddSingleton<PermissionGrantStrategyFactory>();
        context.Services.AddTransient<IPermissionManager, PermissionManager>();
    }
}
