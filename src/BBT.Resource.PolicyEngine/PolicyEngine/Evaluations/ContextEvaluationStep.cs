namespace BBT.Resource.PolicyEngine.Evaluations;

internal sealed class ContextEvaluationStep : IEvaluationStep
{
    public Task<bool> EvaluateAsync(IPolicy policy, UserRequestContext context)
    {
        // TODO: Need implementation analysis
        return Task.FromResult(true);
    }
}
