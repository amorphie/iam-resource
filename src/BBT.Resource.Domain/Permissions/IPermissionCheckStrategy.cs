using System.Threading;
using System.Threading.Tasks;

namespace BBT.Resource.Permissions;

public interface IPermissionCheckStrategy
{
    Task<bool> CheckAsync(
        string applicationId,
        string clientId,
        string providerKey,
        string permissionName,
        CancellationToken cancellationToken = default);
}
