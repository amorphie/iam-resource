using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BBT.Resource.Permissions.GrantProviders;

internal sealed class UserPermissionGrantStrategy(
    IPermissionGrantRepository permissionGrantRepository,
    IHttpContextAccessor httpContextAccessor)
    : BasePermissionGrantStrategy(permissionGrantRepository), IPermissionGrantStrategy
{
    public override async Task<List<PermissionGrantInfo>> GetAsync(
        string applicationId,
        string clientId,
        string providerName,
        string providerKey,
        CancellationToken cancellationToken = default)
    {
        var grantedPermissions = new List<PermissionGrantInfo>();
        
        var userPermissions = await _permissionGrantRepository.GetListAsync(
            applicationId, clientId, providerName, providerKey, cancellationToken);

        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext != null)
        {
            var roles = httpContext.ToRoles()?.Split(",");
            if (roles != null)
                foreach (var role in roles)
                {
                    var rolePermissions = await permissionGrantRepository.GetListAsync(
                        applicationId, clientId, "R", role, cancellationToken);

                    grantedPermissions.AddRange(rolePermissions.Select(rp => new PermissionGrantInfo
                    {
                        Name = rp.Name,
                        IsGranted = true,
                        Provider = "R",
                        ParentName = Permission.ExtractParentName(rp.Name)
                    }));
                }
        }
        
        grantedPermissions.AddRange(userPermissions.Select(up => new PermissionGrantInfo
        {
            Name = up.Name,
            IsGranted = true,
            Provider = "U",
            ParentName = Permission.ExtractParentName(up.Name)
        }));

        return grantedPermissions;
    }
}
