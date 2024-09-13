namespace BBT.Resource.PolicyEngine;

public interface IPolicy
{
    public string Id { get; set; }
    public string Name { get; set; }
    /// <summary>
    /// This field allows a policy to inherit from another, more general policy. If the child policy is more specific, it will override the parent policy's conditions or effects.
    /// </summary>
    public string ParentId { get; set; }
    public Conditions Conditions { get; set; }
    /// <summary>
    /// Policies include permissions to determine what actions are allowed or denied based on evaluated conditions.
    /// </summary>
    public List<string> Permissions { get; set; }
    /// <summary>
    /// The effect field determines the outcome of the policy (allow or deny).
    /// </summary>
    public string Effect { get; set; } // "allow" or "deny"
    /// <summary>
    ///  The evaluationOrder field defines the sequence in which conditions are evaluated, helping to manage conflicts and dependencies.
    /// </summary>
    public List<string> EvaluationOrder { get; set; }
    /// <summary>
    /// Policies can be assigned a priority level. When multiple policies apply, those with higher priority (lower numerical value) are evaluated first.
    /// </summary>
    public int Priority { get; set; }
    /// <summary>
    /// Defines the strategy for resolving conflicts between multiple applicable policies
    /// </summary>
    public string ConflictResolution { get; set; }
}
