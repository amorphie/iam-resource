namespace BBT.Resource.PolicyEngine;

public interface IPolicy
{
    /// <summary>
    /// Policy id
    /// </summary>
    public string Id { get; set; }
    /// <summary>
    /// Policy name
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// This field allows a policy to inherit from another, more general policy. If the child policy is more specific, it will override the parent policy's conditions or effects.
    /// </summary>
    public string ParentId { get; set; }
    /// <summary>
    /// Conditions
    /// </summary>
    public Conditions Conditions { get; set; }
    /// <summary>
    /// Policies include permissions to determine what actions are allowed or denied based on evaluated conditions.
    /// </summary>
    public List<string> Permissions { get; set; }
    /// <summary>
    /// Permission Provider Name (User: U, Role: R, Client: C)
    /// </summary>
    public string? PermissionProvider { get; set; }
    /// <summary>
    /// The effect field determines the outcome of the policy (allow or deny).
    /// </summary>
    public string Effect { get; set; }
    /// <summary>
    ///  The evaluationOrder field defines the sequence in which conditions are evaluated, helping to manage conflicts and dependencies.
    /// </summary>
    /// <remarks>
    /// Evaluation types:
    /// - roles
    /// - attributes
    /// - context
    /// - rules
    /// - time
    ///
    /// Default value: ["roles", "attributes", "context", "rules", "time"]
    /// </remarks>
    public List<string> EvaluationOrder { get; set; }
    /// <summary>
    /// Policies can be assigned a priority level. When multiple policies apply, those with higher priority (lower numerical value) are evaluated first.
    /// </summary>
    public int Priority { get; set; }
    /// <summary>
    /// Defines the strategy for resolving conflicts between multiple applicable policies
    /// </summary>
    /// <remarks>
    /// Conflict resolution strategies:
    /// - <c>N</c>: Not applicable (N/A).
    /// - <c>D</c>: Deny-overrides - Deny policies take precedence over allow policies.
    /// - <c>A</c>: Allow-overrides - Allow policies take precedence over deny policies.
    /// - <c>F</c>: First-applicable - The first applicable policy in the evaluation order is enforced.
    /// - <c>H</c>: Highest-priority - The policy with the highest priority (lowest numerical value) is enforced.
    /// </remarks>
    public string ConflictResolution { get; set; }
}
