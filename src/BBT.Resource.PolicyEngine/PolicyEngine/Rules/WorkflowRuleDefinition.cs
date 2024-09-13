namespace BBT.Resource.PolicyEngine.Rules;

internal class WorkflowRuleDefinition(string name, RuleDefinition[] rules)
{
    public string WorkflowName { get; } = name;
    public RuleDefinition[] Rules { get; } = rules;
}
