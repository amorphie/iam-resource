using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BBT.Resource.Permissions;

public interface IPermissionManager
{
    Task<bool> CheckAsync(
        string applicationId,
        string clientId,
        string providerName,
        string providerKey,
        string permissionName,
        CancellationToken cancellationToken = default);

    Task<bool> CheckMultipleAsync(
        string applicationId,
        string clientId,
        string providerName,
        string providerKey,
        string[] permissionNames,
        CancellationToken cancellationToken = default);

    Task DenyAsync(
        PermissionGrant permission,
        CancellationToken cancellationToken = default);

    Task GrantAsync(
        PermissionGrant permission,
        CancellationToken cancellationToken = default);

    Task BulkGrantAsync(
        IEnumerable<PermissionGrant> permissions,
        CancellationToken cancellationToken = default);

    Task BulkDenyAsync(
        IEnumerable<PermissionGrant> permissions,
        CancellationToken cancellationToken = default);
}
