namespace BBT.Resource.PolicyEngine.ConflictResolutions;

public interface IConflictResolutionStrategy
{
    EvaluationResult Resolve(List<EvaluationResult> evaluationResults);
}
