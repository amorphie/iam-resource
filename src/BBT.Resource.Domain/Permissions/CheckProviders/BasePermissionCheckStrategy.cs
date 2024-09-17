using System.Threading;
using System.Threading.Tasks;

namespace BBT.Resource.Permissions.CheckProviders;

internal abstract class BasePermissionCheckStrategy(IPermissionGrantRepository permissionGrantRepository)
{
    protected abstract string ProviderName { get; }
    protected readonly IPermissionGrantRepository _permissionGrantRepository = permissionGrantRepository;
    
    public virtual async Task<bool> CheckAsync(
        string applicationId,
        string clientId,
        string clientKey,
        string permissionName,
        CancellationToken cancellationToken = default)
    {
        var clientPermission = await _permissionGrantRepository.FindAsync(applicationId, clientId, ProviderName, clientKey,
            permissionName, cancellationToken);
        return clientPermission != null;
    }
}
