using BBT.Prism.Application;
using BBT.Prism.Modularity;

namespace BBT.Resource;

[Modules(
    typeof(ResourceDomainSharedModule),
    typeof(PrismDddApplicationContractsModule)
)]
public class ResourceApplicationContractsModule : PrismModule
{

}