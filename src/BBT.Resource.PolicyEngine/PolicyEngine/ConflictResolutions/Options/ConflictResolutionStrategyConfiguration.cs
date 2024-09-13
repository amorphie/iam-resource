using BBT.Prism;

namespace BBT.Resource.PolicyEngine.ConflictResolutions.Options;

public class ConflictResolutionStrategyConfiguration(
    string name,
    string displayName,
    Type strategyProviderType,
    string description = "",
    bool isDefault = false,
    int order = 1000)
{
    public string Name { get; } = Check.NotNullOrWhiteSpace(name, nameof(name));
    public string DisplayName { get; set; } = displayName;
    public string Description { get; set; } = description;
    public Type Provider { get; } = strategyProviderType;
    public bool Default { get; set; } = isDefault;

    /// <summary>
    /// Default value: 1000.
    /// </summary>
    public int Order { get; set; } = order;
}
