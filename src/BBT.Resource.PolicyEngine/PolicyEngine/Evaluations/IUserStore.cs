namespace BBT.Resource.PolicyEngine.Evaluations;

public interface IUserStore
{
    Task<List<string>> GetUserPermissionsAsync(string userId);
}
