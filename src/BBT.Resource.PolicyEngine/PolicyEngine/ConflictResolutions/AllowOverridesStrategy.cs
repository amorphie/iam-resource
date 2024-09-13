namespace BBT.Resource.PolicyEngine.ConflictResolutions;

internal sealed class AllowOverridesStrategy : IConflictResolutionStrategy
{
    public EvaluationResult Resolve(List<EvaluationResult> evaluationResults)
    {
        var result = new EvaluationResult();
        
        // Eğer herhangi bir allow varsa, allow uygulanır
        var allowPolicy = evaluationResults.FirstOrDefault(r => r.IsAllowed);
        if (allowPolicy != null)
        {
            result.IsAllowed = true;
            result.PassedConditions = allowPolicy.PassedConditions;
        }
        else
        {
            result.IsAllowed = false;
            result.FailedConditions = evaluationResults.SelectMany(r => r.FailedConditions).ToList();
            result.FailureReason = "Allow overrides applied";
        }

        return result;
    }
}
