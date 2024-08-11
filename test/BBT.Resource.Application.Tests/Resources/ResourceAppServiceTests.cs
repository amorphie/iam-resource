using System;
using System.Threading.Tasks;
using BBT.Prism.Modularity;
using Shouldly;
using Xunit;

namespace BBT.Resource.Resources;

public abstract class ResourceAppServiceTests<TStartupModule> : ResourceApplicationTestBase<TStartupModule>
    where TStartupModule : IPrismModule
{
    private readonly IResourceAppService _appService;
    private readonly TestData _testData;

    protected ResourceAppServiceTests()
    {
        _appService = GetRequiredService<IResourceAppService>();
        _testData = GetRequiredService<TestData>();
    }
    
    [Fact]
    public async Task CreateAsync_ShouldReturnCreatedResource()
    {
        var input = new CreateResourceInput([
            new() { Language = "en-US", Name = "Role" },
            new() { Language = "tr-TR", Name = "Rol" }
        ])
        {
            Id = Guid.NewGuid(),
            Url = "/api/roles/([^/]+)/assigned",
            Type = ResourceType.GET,
            Tags = ["roles"]
        };

        var response = await _appService.CreateAsync(input);
        response.ShouldNotBeNull();
        response.Id.ShouldBe(input.Id);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedResource()
    {
        var input = new UpdateResourceInput([
            new() { Language = "en-US", Name = "Get User", Description = "Get user by id"},
            new() { Language = "tr-TR", Name = "Kullanıcı Getir", Description = "Id'ye göre kullanıcı getir"}
        ])
        {
            Status = Status.Active.Code,
            Url = "/v1/api/users/([^/]+)",
            Type = ResourceType.GET,
            Tags = ["user", "users"]
        };
        var response = await _appService.UpdateAsync(_testData.ResourceId_1, input);
        response.ShouldNotBeNull();
        response.Tags?.Length.ShouldBeGreaterThanOrEqualTo(2);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnResourceDto()
    {
        var response = await _appService.GetAsync(_testData.ResourceId_1);

        response.ShouldNotBeNull();
        response.Id.ShouldBe(_testData.ResourceId_1);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnPagedResult()
    {
        var pagedInput = new PagedResourceInput()
        {
            SkipCount = 0,
            MaxResultCount = 1
        };
        var result = await _appService.GetAllAsync(pagedInput);
        result.ShouldNotBeNull();
        result.Items.ShouldHaveSingleItem();
    }
    
    [Fact]
    public async Task GetRulesAsync_ShouldReturnResourceRuleDto()
    {
        var response = await _appService.GetRulesAsync(_testData.ResourceId_1);
        response.Items.Count.ShouldBeGreaterThanOrEqualTo(1);
    }
    
    [Fact]
    public async Task AddRuleAsync_ShouldAddRule()
    {
        var input = new AddRuleToResourceInput()
        {
            RuleId = _testData.RuleId_2,
            ClientId = Guid.NewGuid(),
            Priority = 1
        };

        await _appService.AddRuleAsync(_testData.ResourceId_2, input);
    }
    
    [Fact]
    public async Task AddRuleAsync_ShouldAddRule_IfRuleExists()
    {
        var input = new AddRuleToResourceInput()
        {
            RuleId = _testData.RuleId_1,
            ClientId = Guid.NewGuid(),
            Priority = 1
        };

        await _appService.AddRuleAsync(_testData.ResourceId_1, input);
    }
    
    [Fact]
    public async Task UpdateRuleAsync_ShouldUpdateRule()
    {
        var input = new UpdateResourceRuleInput()
        {
            Status = Status.Passive.Code,
            Priority = 2
        };

        await _appService.UpdateRuleAsync(_testData.ResourceId_1, _testData.RuleId_1, input);
    }
    
    [Fact]
    public async Task RemoveRuleAsync_ShouldRemoveRule()
    {
        await _appService.RemoveRuleAsync(_testData.ResourceId_1, _testData.RuleId_1);
    }
}
