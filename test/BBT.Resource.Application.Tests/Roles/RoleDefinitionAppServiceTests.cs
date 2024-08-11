using System;
using System.Threading.Tasks;
using BBT.Prism.Modularity;
using Shouldly;
using Xunit;

namespace BBT.Resource.Roles;

public abstract class RoleDefinitionAppServiceTests<TStartupModule> : ResourceApplicationTestBase<TStartupModule>
    where TStartupModule : IPrismModule
{
    private readonly IRoleDefinitionAppService _appService;
    private readonly TestData _testData;

    protected RoleDefinitionAppServiceTests()
    {
        _appService = GetRequiredService<IRoleDefinitionAppService>();
        _testData = GetRequiredService<TestData>();
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCreatedRoleDefinition()
    {
        var input = new CreateRoleDefinitionInput([new() { Language = "en-US", Name = "Editor" }])
        {
            Id = Guid.NewGuid(),
            Key = "Editor",
            ClientId = Guid.NewGuid(),
            Tags = new[] { "editor" }
        };

        var response = await _appService.CreateAsync(input);
        response.ShouldNotBeNull();
        response.Id.ShouldBe(input.Id);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedRoleDefinition()
    {
        var newKey = $"Observer";
        var input = new UpdateRoleDefinitionInput([
            new() { Language = "en-US", Name = "Observer" },
            new() { Language = "tr-TR", Name = "Gözlemci" }
        ])
        {
            Key = newKey,
            Status = Status.Active.Code,
            Tags = []
        };
        var response = await _appService.UpdateAsync(_testData.RoleDefinitionId_2, input);
        response.ShouldNotBeNull();
        response.Key.ShouldBe(newKey);
        response.Translations.Count.ShouldBeGreaterThanOrEqualTo(2);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnRoleDefinitionDto()
    {
        var response = await _appService.GetAsync(_testData.RoleDefinitionId_1);
        response.ShouldNotBeNull();
        response.Id.ShouldBe(_testData.RoleDefinitionId_1);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnPagedResult()
    {
        var pagedInput = new PagedRoleDefinitionInput()
        {
            SkipCount = 0,
            MaxResultCount = 1
        };
        var result = await _appService.GetAllAsync(pagedInput);
        result.ShouldNotBeNull();
        result.Items.ShouldHaveSingleItem();
    }
}
