namespace BBT.Resource.PolicyEngine.Evaluations;

internal sealed class RolesEvaluationStep: IEvaluationStep
{
    public Task<bool> EvaluateAsync(IPolicy policy, UserRequestContext context)
    {
        var result =  policy.Conditions.Roles.Any(role => context.Roles != null && context.Roles.Contains(role));
        return Task.FromResult(result);
    }
}
