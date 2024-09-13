using BBT.Prism.Modularity;

namespace BBT.Resource.PolicyEngine;

public abstract class PolicyEngineTestBase<TStartupModule> : ResourceTestBase<TStartupModule>
    where TStartupModule : IPrismModule
{
}
