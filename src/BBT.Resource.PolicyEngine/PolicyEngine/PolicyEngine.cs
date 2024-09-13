using BBT.Resource.PolicyEngine.ConflictResolutions;
using BBT.Resource.PolicyEngine.Evaluations;

namespace BBT.Resource.PolicyEngine;

internal sealed class PolicyEngine(
    EvaluationManager evaluationManager,
    ConflictResolutionManager conflictResolutionManager)
    : IPolicyEngine
{
    public async Task<EvaluationResult> EvaluatePoliciesAsync(List<IPolicy> policies, UserRequestContext context)
    {
        var sortedPolicies = policies.OrderBy(p => p.Priority).ToList();
        var results = new List<EvaluationResult>();

        foreach (var policy in sortedPolicies)
        {
            var evaluationResult = await evaluationManager.EvaluateAsync(policy, context);
            results.Add(evaluationResult);
        }

        var firstPolicy = sortedPolicies.First();
        return conflictResolutionManager.Resolve(firstPolicy.ConflictResolution, results);
    }
}
