using BBT.Resource.PolicyEngine.Rules;

namespace BBT.Resource.PolicyEngine.Evaluations;

internal sealed class RulesEvaluationStep(IRuleEngine ruleEngine) : IEvaluationStep
{
    public async Task<bool> EvaluateAsync(IPolicy policy, UserRequestContext context)
    {
        var response = await ruleEngine.ExecuteRules(new RuleContext()
        {
            AllHeaders = context.AllHeaders!,
            Data = context.Data,
            RequestUrl = context.RequestUrl!,
            UrlPattern = context.UrlPattern,
            RuleDefinitions = policy
                .Conditions
                .Rules
                .Select(s =>
                    new RuleDefinition(s.Id, s.Expression))
                .ToList()
        });

        return response.All(a => a.IsSuccess);
    }
}
