namespace BBT.Resource.PolicyEngine.ConflictResolutions;

internal sealed class FirstApplicableStrategy : IConflictResolutionStrategy
{
    public EvaluationResult Resolve(List<EvaluationResult> evaluationResults)
    {
        // İlk geçerli (IsAllowed = true) olan politikayı buluyoruz
        var firstApplicable = evaluationResults.FirstOrDefault(r => r.IsAllowed);
        if (firstApplicable != null)
        {
            return firstApplicable;
        }

        // Eğer geçerli bir allow politikası yoksa, ilk deny politikası uygulanır.
        return evaluationResults.FirstOrDefault(r => !r.IsAllowed) ?? new EvaluationResult { IsAllowed = false, FailureReason = "No applicable policy" };
    }
}
