namespace BBT.Resource.PolicyEngine.Evaluations;

public interface IPermissionStore
{
    Task<bool> CheckAsync(
        string applicationId,
        string clientId,
        string providerName,
        string providerKey,
        string[] permissionNames);
}
