using BBT.Resource.Roles;
using Xunit;

namespace BBT.Resource.EntityFrameworkCore.Applications;

[Collection(ResourceTestConsts.CollectionDefinitionName)]
public class EfCoreRoleGroupAppServiceTests: RoleGroupAppServiceTests<ResourceEntityFrameworkCoreTestModule>
{
    
}
