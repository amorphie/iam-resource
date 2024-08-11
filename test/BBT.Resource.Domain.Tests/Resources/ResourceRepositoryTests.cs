using System.Threading.Tasks;
using BBT.Prism.Modularity;
using Shouldly;
using Xunit;

namespace BBT.Resource.Resources;

public abstract class ResourceRepositoryTests<TStartupModule> : ResourceDomainTestBase<TStartupModule>
    where TStartupModule : IPrismModule
{
    private readonly IResourceRepository _repository;
    private readonly TestData _testData;

    protected ResourceRepositoryTests()
    {
        _repository = GetRequiredService<IResourceRepository>();
        _testData = GetRequiredService<TestData>();
    }
    
    [Fact]
    public async Task GetWithRuleAsync_ShouldReturnResourceWithRule()
    {
        var response = await _repository.GetWithRuleAsync(_testData.ResourceId_1);
        response.ShouldNotBeNull();
        response.Rules.ShouldNotBeNull();
        response.Rules.Count.ShouldBeGreaterThanOrEqualTo(1);
    }
    
    [Fact]
    public async Task GetRulesAsync_ShouldReturnResourceRuleModelItems()
    {
        var response = await _repository.GetRulesAsync(_testData.ResourceId_1);
        response.ShouldNotBeNull();
        response.Count.ShouldBeGreaterThanOrEqualTo(1);
    }
    
    [Fact]
    public async Task GetRuleAsync_ShouldReturnResourceRuleModel()
    {
        var response = await _repository.GetRuleAsync(_testData.ResourceId_1, _testData.RuleId_1);
        response.ShouldNotBeNull();
        response.Rule.Id.ShouldBe(_testData.RuleId_1);
        response.RelatedRule.ResourceId.ShouldBe(_testData.ResourceId_1);
    }
    
    [Fact]
    public async Task FindByRegexAsync_ShouldMatchUrl()
    {
        var response = await _repository.FindByRegexAsync("/api/users/25", ResourceType.GET);
        response.ShouldNotBeNull();
    }
    
    [Fact]
    public async Task FindByRegexAsync_ShouldReturnNullWhenNotMatch()
    {
        var response = await _repository.FindByRegexAsync("/api/users/25", ResourceType.POST);
        response.ShouldBeNull();
    }
}
