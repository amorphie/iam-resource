using BBT.Prism;
using BBT.Prism.Modularity;
using BBT.Prism.Testing;

namespace BBT.Resource;

/* All test classes are derived from this class, directly or indirectly. */
public abstract class ResourceTestBase<TStartupModule>: PrismIntegratedTest<TStartupModule>
    where TStartupModule : IPrismModule
{
    protected override void SetPrismApplicationCreationOptions(PrismApplicationCreationOptions options)
    {
    }
}
