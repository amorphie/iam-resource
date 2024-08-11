using BBT.Prism.Modularity;

namespace BBT.Resource;

public abstract class ResourceDomainTestBase<TStartupModule> : ResourceTestBase<TStartupModule>
    where TStartupModule : IPrismModule
{
}
