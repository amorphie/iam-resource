using System;
using BBT.Resource.Permissions.GrantProviders;
using Microsoft.Extensions.DependencyInjection;

namespace BBT.Resource.Permissions;

public class PermissionGrantStrategyFactory(IServiceProvider serviceProvider)
{
    public IPermissionGrantStrategy CreateStrategy(string providerName)
    {
        return providerName switch
        {
            "U" => serviceProvider.GetRequiredService<UserPermissionGrantStrategy>(),
            _ => serviceProvider.GetRequiredService<DefaultPermissionGrantStrategy>()
        };
    }
}
