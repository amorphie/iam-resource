using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BBT.Resource.Permissions.GrantProviders;

internal abstract class BasePermissionGrantStrategy(IPermissionGrantRepository permissionGrantRepository)
    : IPermissionGrantStrategy
{
    protected readonly IPermissionGrantRepository _permissionGrantRepository = permissionGrantRepository;

    public virtual async Task<List<PermissionGrantInfo>> GetAsync(
        string applicationId,
        string clientId,
        string providerName,
        string providerKey,
        CancellationToken cancellationToken = default)
    {
        var grantedPermissions = new List<PermissionGrantInfo>();

        var permissions = await _permissionGrantRepository.GetListAsync(
            applicationId, clientId, providerName, providerKey, cancellationToken);

        grantedPermissions.AddRange(permissions.Select(up => new PermissionGrantInfo
        {
            Name = up.Name,
            IsGranted = true,
            Provider = providerName,
            ParentName = Permission.ExtractParentName(up.Name)
        }));

        return grantedPermissions;
    }
}
