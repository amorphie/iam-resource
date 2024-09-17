namespace BBT.Resource.PolicyEngine.ConflictResolutions;

internal sealed class HighestPriorityStrategy : IConflictResolutionStrategy
{
    public EvaluationResult Resolve(List<EvaluationResult> evaluationResults)
    {
        var result = evaluationResults.OrderBy(r => r.FailedConditions.Count).First(); // Sıralama mantığına göre en yüksek öncelik
        return result;
    }
}
