using BBT.Prism.Modularity;

namespace BBT.Resource;

[Modules(
    typeof(ResourceDomainModule),
    typeof(ResourceTestBaseModule)
)]
public class ResourceDomainTestModule: PrismModule
{
    
}