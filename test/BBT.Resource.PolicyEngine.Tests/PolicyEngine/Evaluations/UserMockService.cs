namespace BBT.Resource.PolicyEngine.Evaluations;

public class UserMockService : IUserStore
{
    public Task<List<string>> GetUserPermissionsAsync(string userId)
    {
        return Task.FromResult(new List<string>()
        {
            "Users",
            "Users.Create",
            "Users.Update",
            "Users.Delete"
        });
    }
}
