using BBT.Resource.Permissions;
using Xunit;

namespace BBT.Resource.EntityFrameworkCore.Applications;

[Collection(ResourceTestConsts.CollectionDefinitionName)]
public class EfCorePermissionAppServiceTests: PermissionAppServiceTests<ResourceEntityFrameworkCoreTestModule>
{
    
}
