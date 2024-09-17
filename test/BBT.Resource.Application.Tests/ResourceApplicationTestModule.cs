using BBT.Prism.Modularity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace BBT.Resource;

[Modules(
    typeof(ResourceApplicationModule),
    typeof(ResourceDomainTestModule)
)]
public class ResourceApplicationTestModule : PrismModule
{
    
}
