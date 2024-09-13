using RulesEngine.Models;

namespace BBT.Resource.PolicyEngine.Rules;

public interface IRuleEngine
{
    Task<List<RuleResultTree>> ExecuteRules(RuleContext ruleContext);
}
