using System.Threading.Tasks;
using BBT.Prism.Modularity;
using Shouldly;
using Xunit;

namespace BBT.Resource.Roles;

public abstract class RoleGroupRepositoryTests<TStartupModule> : ResourceDomainTestBase<TStartupModule>
    where TStartupModule : IPrismModule
{
    private readonly IRoleGroupRepository _repository;
    private readonly TestData _testData;

    protected RoleGroupRepositoryTests()
    {
        _repository = GetRequiredService<IRoleGroupRepository>();
        _testData = GetRequiredService<TestData>();
    }

    [Fact]
    public async Task GetRolesAsync_ShouldReturnRoleGroupRelatedRoleModelItems()
    {
        var response = await _repository.GetRolesAsync(_testData.RoleGroupId_1);
        response.ShouldNotBeNull();
        response.Count.ShouldBeGreaterThanOrEqualTo(1);
    }
    
    [Fact]
    public async Task GetRoleAsync_ShouldReturnRoleGroupRelatedRoleModel()
    {
        var response = await _repository.GetRoleAsync(_testData.RoleGroupId_1, _testData.RoleId_1);
        response.ShouldNotBeNull();
        response.Role.Id.ShouldBe(_testData.RoleId_1);
        response.RelatedRole.GroupId.ShouldBe(_testData.RoleGroupId_1);
    }
}
