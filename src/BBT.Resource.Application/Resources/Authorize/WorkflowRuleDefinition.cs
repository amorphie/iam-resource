namespace BBT.Resource.Resources.Authorize;

public class WorkflowRuleDefinition(string name, RuleDefinition[] rules)
{
    public string WorkflowName { get; } = name;
    public RuleDefinition[] Rules { get; } = rules;
}
