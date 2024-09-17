using BBT.Prism.Modularity;

namespace BBT.Resource.Policies;

public abstract class PolicyRepositoryTests<TStartupModule> : ResourceDomainTestBase<TStartupModule>
    where TStartupModule : IPrismModule;
