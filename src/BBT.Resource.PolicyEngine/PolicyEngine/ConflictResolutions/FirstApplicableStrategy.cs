namespace BBT.Resource.PolicyEngine.ConflictResolutions;

internal sealed class FirstApplicableStrategy : IConflictResolutionStrategy
{
    public EvaluationResult Resolve(List<EvaluationResult> evaluationResults)
    {
        var firstApplicable = evaluationResults.FirstOrDefault(r => r.IsAllowed);
        if (firstApplicable != null)
        {
            return firstApplicable;
        }
        
        return evaluationResults.FirstOrDefault(r => !r.IsAllowed) ?? new EvaluationResult { IsAllowed = false, FailureReason = "No applicable policy" };
    }
}
