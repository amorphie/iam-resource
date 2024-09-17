namespace BBT.Resource.PolicyEngine.Evaluations;

public class PermissionMockService : IPermissionStore
{
    public Task<bool> CheckAsync(
        string applicationId,
        string clientId,
        string providerName,
        string providerKey,
        string[] permissionNames)
    {
        var list = new List<string>()
        {
            "Users",
            "Users.Create",
            "Users.Update",
            "Users.Delete"
        };
        bool allPermissionsExist = permissionNames.All(permission => list.Contains(permission));
        return Task.FromResult(allPermissionsExist);
    }
}
