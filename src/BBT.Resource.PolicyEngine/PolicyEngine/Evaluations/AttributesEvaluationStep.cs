namespace BBT.Resource.PolicyEngine.Evaluations;

internal sealed class AttributesEvaluationStep: IEvaluationStep
{
    public Task<bool> EvaluateAsync(IPolicy policy, UserRequestContext context)
    {
        var result =  policy.Conditions.Attributes.All(attr => 
            context.Attributes != null && context.Attributes.ContainsKey(attr.Key) && context.Attributes[attr.Key] == attr.Value);
        return Task.FromResult(result);
    }
}
