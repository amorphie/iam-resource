namespace BBT.Resource.PolicyEngine;

public interface IPolicyEngine
{
    Task<EvaluationResult> EvaluatePoliciesAsync(List<IPolicy> policies, UserRequestContext context);
}
