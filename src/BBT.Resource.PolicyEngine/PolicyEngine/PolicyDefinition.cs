namespace BBT.Resource.PolicyEngine;

public class PolicyDefinition : IPolicy
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string ParentId { get; set; }
    public Conditions Conditions { get; set; }
    public List<string>? Permissions { get; set; }
    public string? PermissionProvider { get; set; }
    public string Effect { get; set; } // "allow" or "deny"
    public List<string> EvaluationOrder { get; set; }
    public int Priority { get; set; }
    public string ConflictResolution { get; set; }

    public void MergeWithParent(PolicyDefinition parentPolicy)
    {
        Conditions = MergeConditions(parentPolicy.Conditions, Conditions);
        
        Permissions = MergePermissions(parentPolicy.Permissions, Permissions);
        
        EvaluationOrder = parentPolicy.EvaluationOrder.Concat(EvaluationOrder).ToList();
        ConflictResolution = parentPolicy.ConflictResolution;
    }
    
    private Conditions MergeConditions(Conditions parentConditions, Conditions childConditions)
    {
        var mergedRules = parentConditions.Rules.Concat(childConditions.Rules).ToList();
        
        return new Conditions
        {
            Attributes = MergeDictionaries(parentConditions.Attributes, childConditions.Attributes),
            Roles = MergeLists(parentConditions.Roles, childConditions.Roles),
            Context = MergeDictionaries(parentConditions.Context, childConditions.Context),
            Rules = mergedRules,
            Time = childConditions.Time ?? parentConditions.Time
        };
    }
    
    private List<string> MergePermissions(List<string> parentPermissions, List<string> childPermissions)
    {
        // Parent permissions'ın child permissions'a önceliği olacak, unique olarak birleştir
        var mergedPermissions = parentPermissions.Concat(childPermissions ?? new List<string>())
            .Distinct()
            .ToList();
        return mergedPermissions;
    }
    
    private Dictionary<string, string> MergeDictionaries(Dictionary<string, string> parentDict, Dictionary<string, string> childDict)
    {
        var result = new Dictionary<string, string>(parentDict);

        foreach (var kvp in childDict)
        {
            if (!result.ContainsKey(kvp.Key))
            {
                result[kvp.Key] = kvp.Value;
            }
        }

        return result;
    }

    private List<string> MergeLists(List<string> parentList, List<string> childList)
    {
        return parentList.Concat(childList ?? new List<string>()).Distinct().ToList();
    }
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
    public RuleCondition()
    {
        
    }

    public RuleCondition(string id, string expression)
    {
        Id = id;
        Expression = expression;
    }
    public string Id { get; set; }
    public string Expression { get; set; } // Rule logic to be evaluated
}

public class TimeCondition
{
    public string Start { get; set; }
    public string End { get; set; }
    public string Timezone { get; set; }
}
