namespace BBT.Resource.PolicyEngine.ConflictResolutions;

internal sealed class DenyOverridesStrategy : IConflictResolutionStrategy
{
    public EvaluationResult Resolve(List<EvaluationResult> evaluationResults)
    {
        var result = new EvaluationResult();
        
        // Eğer herhangi bir deny varsa, deny uygulanır
        var denyPolicy = evaluationResults.FirstOrDefault(r => !r.IsAllowed);
        if (denyPolicy != null)
        {
            result.IsAllowed = false;
            result.FailedConditions = denyPolicy.FailedConditions;
            result.FailureReason = "Deny overrides applied";
        }
        else
        {
            result.IsAllowed = true;
            result.PassedConditions = evaluationResults.SelectMany(r => r.PassedConditions).ToList();
        }

        return result;
    }
}
