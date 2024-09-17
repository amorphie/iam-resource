using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BBT.Resource.Permissions.CheckProviders;

internal sealed class ClientPermissionCheckStrategy(
    IPermissionGrantRepository permissionGrantRepository,
    IHttpContextAccessor httpContextAccessor)
    : BasePermissionCheckStrategy(permissionGrantRepository), IPermissionCheckStrategy
{
    protected override string ProviderName => "C";

    public override async Task<bool> CheckAsync(string applicationId, string clientId, string clientKey,
        string permissionName,
        CancellationToken cancellationToken = default)
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext == null) return false;
        var requestClientId = httpContext.GetClientId();
        if (requestClientId.IsNullOrEmpty()) return false;
        var permission = await _permissionGrantRepository.FindAsync(applicationId, clientId, ProviderName,
            requestClientId, permissionName, cancellationToken);

        if (permission != null)
        {
            return true;
        }

        return false;
    }
}
