namespace BBT.Resource.PolicyEngine.Evaluations;

public interface IEvaluationStep
{
    Task<bool> EvaluateAsync(IPolicy policy, UserRequestContext context);
}
