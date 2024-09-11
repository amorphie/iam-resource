using BBT.Resource.Policies;
using Xunit;

namespace BBT.Resource.EntityFrameworkCore.Applications;

[Collection(ResourceTestConsts.CollectionDefinitionName)]
public class EfCorePolicyAppServiceTests: PolicyAppServiceTests<ResourceEntityFrameworkCoreTestModule>
{
    
}
