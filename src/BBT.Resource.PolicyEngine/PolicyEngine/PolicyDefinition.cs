namespace BBT.Resource.PolicyEngine;

public class PolicyDefinition : IPolicy
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string ParentId { get; set; }
    public Conditions Conditions { get; set; }
    public List<string> Permissions { get; set; }
    public string Effect { get; set; } // "allow" or "deny"
    public List<string> EvaluationOrder { get; set; }
    public int Priority { get; set; }
    public string ConflictResolution { get; set; }
}

public class Conditions 
{
    public Dictionary<string, string> Attributes { get; set; }
    public List<string> Roles { get; set; }
    public Dictionary<string, string> Context { get; set; }
    public List<RuleCondition> Rules { get; set; }
    public TimeCondition Time { get; set; }
}

public class RuleCondition
{
    public string Id { get; set; }
    public string Expression { get; set; } // Rule logic to be evaluated
}

public class TimeCondition
{
    public string Start { get; set; }
    public string End { get; set; }
    public string Timezone { get; set; }
}
