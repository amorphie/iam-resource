namespace BBT.Resource.PolicyEngine.ConflictResolutions.Options;

public class ConflictResolutionConfigurationDictionary : Dictionary<string, ConflictResolutionStrategyConfiguration>
{
    public void Add(ConflictResolutionStrategyConfiguration strategyConfiguration)
    {
        this[strategyConfiguration.Name] = strategyConfiguration;
    }
}
