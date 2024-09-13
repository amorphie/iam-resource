using BBT.Prism;

namespace BBT.Resource.PolicyEngine.Evaluations.Options;

public class EvaluationStepConfiguration(
    string name,
    string displayName,
    Type stepProviderType,
    int order = 1000)
{
    public string Name { get; } = Check.NotNullOrWhiteSpace(name, nameof(name));
    public string DisplayName { get; set; } = displayName;
    public Type Provider { get; } = stepProviderType;

    /// <summary>
    /// Default value: 1000.
    /// </summary>
    public int Order { get; set; } = order;
}
