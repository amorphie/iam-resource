using System.Collections.Generic;
using System.Threading.Tasks;
using BBT.Resource.Permissions;
using BBT.Resource.PolicyEngine.Evaluations;

namespace BBT.Resource.Resources.Authorize;

public class ExternalPermissionStore(IPermissionManager permissionManager) : IPermissionStore
{
    public async Task<bool> CheckAsync(
        string applicationId,
        string clientId,
        string providerName,
        string providerKey,
        string[] permissionNames)
    {
        return await permissionManager.CheckMultipleAsync(
            applicationId,
            clientId,
            providerName,
            providerKey,
            permissionNames
        );
    }
}
