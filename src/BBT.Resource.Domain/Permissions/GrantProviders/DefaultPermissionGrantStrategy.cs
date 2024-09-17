namespace BBT.Resource.Permissions.GrantProviders;

internal sealed class DefaultPermissionGrantStrategy(IPermissionGrantRepository permissionGrantRepository)
    : BasePermissionGrantStrategy(permissionGrantRepository), IPermissionGrantStrategy;
