

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BBT.Resource.Permissions.CheckProviders;

internal sealed class RolePermissionCheckStrategy(
    IPermissionGrantRepository permissionGrantRepository,
    IHttpContextAccessor httpContextAccessor)
    : BasePermissionCheckStrategy(permissionGrantRepository), IPermissionCheckStrategy
{
    protected override string ProviderName => "R";

    public override async Task<bool> CheckAsync(string applicationId, string clientId, string clientKey, string permissionName,
        CancellationToken cancellationToken = default)
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext == null) return false;
        var roles = httpContext.ToRoles()?.Split(",");
        if (roles == null) return false;
        foreach (var role in roles)
        {
            var rolePermission = await _permissionGrantRepository.FindAsync(applicationId, clientId, ProviderName,
                role, permissionName, cancellationToken);
            if (rolePermission != null)
            {
                return true;
            }
        }

        return false;
    }
}
