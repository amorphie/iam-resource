using System;
using System.Threading.Tasks;
using BBT.Prism.Modularity;
using Shouldly;
using Xunit;

namespace BBT.Resource.Rules;

public abstract class RuleAppServiceTests<TStartupModule> : ResourceApplicationTestBase<TStartupModule>
    where TStartupModule : IPrismModule
{
    private readonly IRuleAppService _appService;
    private readonly TestData _testData;

    protected RuleAppServiceTests()
    {
        _appService = GetRequiredService<IRuleAppService>();
        _testData = GetRequiredService<TestData>();
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCreatedRule()
    {
        var input = new CreateRuleInput()
        {
            Id = Guid.NewGuid(),
            Name = "Rule_99",
            Expression = "header.role == \"FullAuthorized\" && header.clientAuthorized == null"
        };

        var response = await _appService.CreateAsync(input);
        response.ShouldNotBeNull();
        response.Id.ShouldBe(input.Id);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedRule()
    {
        var name = $"Rule_{DateTime.Now.Ticks}";
        var input = new UpdateRuleInput()
        {
            Name = name,
            Expression = "header.user_reference == 111111111"
        };
        var response = await _appService.UpdateAsync(_testData.RuleId_1, input);
        response.ShouldNotBeNull();
        response.Name.ShouldBe(name);
        response.Expression.ShouldBe(input.Expression);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnRuleDto()
    {
        var response = await _appService.GetAsync(_testData.RuleId_1);

        response.ShouldNotBeNull();
        response.Id.ShouldBe(_testData.RuleId_1);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnPagedResult()
    {
        var pagedInput = new PagedRuleInput()
        {
            SkipCount = 0,
            MaxResultCount = 1
        };
        var result = await _appService.GetAllAsync(pagedInput);
        result.ShouldNotBeNull();
        result.Items.ShouldHaveSingleItem();
    }
}
