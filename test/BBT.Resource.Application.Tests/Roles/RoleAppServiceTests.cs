using System;
using System.Threading.Tasks;
using BBT.Prism.Modularity;
using Shouldly;
using Xunit;

namespace BBT.Resource.Roles;

public abstract class RoleAppServiceTests<TStartupModule> : ResourceApplicationTestBase<TStartupModule>
    where TStartupModule : IPrismModule
{
    private readonly IRoleAppService _appService;
    private readonly TestData _testData;

    protected RoleAppServiceTests()
    {
        _appService = GetRequiredService<IRoleAppService>();
        _testData = GetRequiredService<TestData>();
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCreatedRole()
    {
        var input = new CreateRoleInput([new() { Language = "en-US", Name = "Accounter" }])
        {
            Id = Guid.NewGuid(),
            DefinitionId = _testData.RoleDefinitionId_1,
            Tags = new[] { "accounter" }
        };

        var response = await _appService.CreateAsync(input);
        response.ShouldNotBeNull();
        response.Id.ShouldBe(input.Id);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedRole()
    {
        var input = new UpdateRoleInput([
            new() { Language = "en-US", Name = "Observer" },
            new() { Language = "tr-TR", Name = "Gözlemci" }
        ])
        {
            Status = Status.Active.Code,
            Tags = ["observer", "gözlemci"]
        };
        var response = await _appService.UpdateAsync(_testData.RoleId_2, input);
        response.ShouldNotBeNull();
        response.Translations.Count.ShouldBeGreaterThanOrEqualTo(2);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnRoleDto()
    {
        var response = await _appService.GetAsync(_testData.RoleId_1);

        response.ShouldNotBeNull();
        response.Id.ShouldBe(_testData.RoleId_1);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnPagedResult()
    {
        var pagedInput = new PagedRoleInput()
        {
            SkipCount = 0,
            MaxResultCount = 1
        };
        var result = await _appService.GetAllAsync(pagedInput);
        result.ShouldNotBeNull();
        result.Items.ShouldHaveSingleItem();
    }
}
