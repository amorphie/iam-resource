using BBT.Resource.Resources;
using Xunit;

namespace BBT.Resource.EntityFrameworkCore.Applications;

[Collection(ResourceTestConsts.CollectionDefinitionName)]
public class EfCoreResourceAuthorizeAppServiceTests: ResourceAuthorizeAppServiceTests<ResourceEntityFrameworkCoreTestModule>
{
    
}