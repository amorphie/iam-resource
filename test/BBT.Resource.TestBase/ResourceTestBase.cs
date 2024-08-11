using BBT.Prism;
using BBT.Prism.Modularity;
using BBT.Prism.Testing;

namespace BBT.Resource;

public abstract class ResourceTestBase<TStartupModule>: PrismIntegratedTest<TStartupModule>
    where TStartupModule : IPrismModule
{
    protected override void SetPrismApplicationCreationOptions(PrismApplicationCreationOptions options)
    {
    }
}
