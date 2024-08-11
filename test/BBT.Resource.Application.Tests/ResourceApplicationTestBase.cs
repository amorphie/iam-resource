using BBT.Prism.Modularity;

namespace BBT.Resource;

public abstract class ResourceApplicationTestBase<TStartupModule> : ResourceTestBase<TStartupModule>
    where TStartupModule : IPrismModule
{
}
