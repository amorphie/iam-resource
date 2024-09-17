using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BBT.Resource.Permissions;

public class PermissionManager(
    IPermissionGrantRepository permissionGrantRepository,
    PermissionCheckStrategyFactory strategyCheckFactory)
    : IPermissionManager
{
    public async Task DenyAsync(PermissionGrant permission, CancellationToken cancellationToken = default)
    {
        await permissionGrantRepository.DeleteAsync(permission, cancellationToken);
    }

    public async Task GrantAsync(PermissionGrant permission, CancellationToken cancellationToken = default)
    {
        await permissionGrantRepository.InsertAsync(permission, cancellationToken);
    }

    public async Task BulkGrantAsync(IEnumerable<PermissionGrant> permissions,
        CancellationToken cancellationToken = default)
    {
        foreach (var permission in permissions)
        {
            await permissionGrantRepository.InsertAsync(permission, cancellationToken);
        }
    }

    public async Task BulkDenyAsync(IEnumerable<PermissionGrant> permissions,
        CancellationToken cancellationToken = default)
    {
        foreach (var permission in permissions)
        {
            await permissionGrantRepository.DeleteAsync(permission, cancellationToken);
        }
    }

    public async Task<bool> CheckAsync(
        string applicationId,
        string clientId,
        string providerName,
        string providerKey,
        string permissionName,
        CancellationToken cancellationToken = default)
    {
        var strategy = strategyCheckFactory.CreateStrategy(providerName);
        var isGranted =
            await strategy.CheckAsync(applicationId, clientId, providerKey, permissionName, cancellationToken);
        
        return isGranted;
    }
    
    public async Task<bool> CheckMultipleAsync(
        string applicationId,
        string clientId,
        string providerName,
        string providerKey,
        string[] permissionNames,
        CancellationToken cancellationToken = default)
    {
        var strategy = strategyCheckFactory.CreateStrategy(providerName);

        foreach (var permissionName in permissionNames)
        {
            var isGranted = await strategy.CheckAsync(applicationId, clientId, providerKey, permissionName, cancellationToken);
            
            if (!isGranted)
            {
                return false;
            }
        }
        
        return true;
    }
}
