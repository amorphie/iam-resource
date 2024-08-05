using BBT.Prism.Modularity;

namespace BBT.Resource;

/* Inherit from this class for your domain layer tests. */
public abstract class ResourceDomainTestBase<TStartupModule> : ResourceTestBase<TStartupModule>
    where TStartupModule: IPrismModule
{

}
