using System.Threading.Tasks;
using BBT.Prism.Modularity;
using Shouldly;
using Xunit;

namespace BBT.Resource.Roles;

public abstract class RoleRepositoryTests<TStartupModule> : ResourceDomainTestBase<TStartupModule>
    where TStartupModule : IPrismModule
{
    private readonly IRoleRepository _repository;
    private readonly TestData _testData;

    protected RoleRepositoryTests()
    {
        _repository = GetRequiredService<IRoleRepository>();
        _testData = GetRequiredService<TestData>();
    }

    [Fact]
    public async Task GetWithDefinitionAsync_ShouldReturnRoleWithDefinitionModel()
    {
        var response = await _repository.GetWithDefinitionAsync(_testData.RoleId_1);
        response.ShouldNotBeNull();
        response.Role.Id.ShouldBe(_testData.RoleId_1);
    }
}
