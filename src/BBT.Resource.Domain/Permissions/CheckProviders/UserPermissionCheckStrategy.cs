using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BBT.Resource.Permissions.CheckProviders;

internal sealed class UserPermissionCheckStrategy(
    IPermissionGrantRepository permissionGrantRepository,
    IHttpContextAccessor httpContextAccessor)
    : BasePermissionCheckStrategy(permissionGrantRepository), IPermissionCheckStrategy
{
    protected override string ProviderName => "U";

    public override async Task<bool> CheckAsync(
        string applicationId,
        string clientId,
        string userId,
        string permissionName,
        CancellationToken cancellationToken = default)
    {
        var userPermission = await _permissionGrantRepository.FindAsync(applicationId, clientId, "U", userId,
            permissionName, cancellationToken);
        if (userPermission != null)
        {
            return true;
        }

        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext == null) return false;
        var roles = httpContext.ToRoles()?.Split(",");
        if (roles == null) return false;
        foreach (var role in roles)
        {
            var rolePermission = await _permissionGrantRepository.FindAsync(applicationId, clientId, "R",
                role, permissionName, cancellationToken);
            if (rolePermission != null)
            {
                return true;
            }
        }

        return false;
    }
}
