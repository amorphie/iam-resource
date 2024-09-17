using System;
using BBT.Resource.Permissions.CheckProviders;
using Microsoft.Extensions.DependencyInjection;

namespace BBT.Resource.Permissions;

public class PermissionCheckStrategyFactory(IServiceProvider serviceProvider)
{
    public IPermissionCheckStrategy CreateStrategy(string providerName)
    {
        return providerName switch
        {
            "U" => serviceProvider.GetRequiredService<UserPermissionCheckStrategy>(),
            "R" => serviceProvider.GetRequiredService<RolePermissionCheckStrategy>(),
            _ => throw new NotSupportedException($"No supported control method found for Provider '{providerName}'.")
        };
    }
}
