using BBT.Prism.Modularity;

namespace BBT.Resource;

[Modules(
    typeof(ResourceApplicationModule),
    typeof(ResourceDomainTestModule)
)]
public class ResourceApplicationTestModule : PrismModule
{

}
