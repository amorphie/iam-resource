using System;
using System.Threading.Tasks;
using BBT.Prism.Modularity;
using Shouldly;
using Xunit;

namespace BBT.Resource.Resources;

public abstract class ResourceGroupAppServiceTests<TStartupModule> : ResourceApplicationTestBase<TStartupModule>
    where TStartupModule : IPrismModule
{
    private readonly IResourceGroupAppService _appService;
    private readonly TestData _testData;

    protected ResourceGroupAppServiceTests()
    {
        _appService = GetRequiredService<IResourceGroupAppService>();
        _testData = GetRequiredService<TestData>();
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCreatedResourceGroup()
    {
        var input = new CreateResourceGroupInput([
            new() { Language = "en-US", Name = "Workflow" },
            new() { Language = "tr-TR", Name = "İş Akışı" }
        ])
        {
            Id = Guid.NewGuid(),
            Tags = ["workflow"]
        };

        var response = await _appService.CreateAsync(input);
        response.ShouldNotBeNull();
        response.Id.ShouldBe(input.Id);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedResourceGroup()
    {
        var input = new UpdateResourceGroupInput([
            new() { Language = "en-US", Name = "Users" },
            new() { Language = "tr-TR", Name = "Kullanıcılar" }
        ])
        {
            Status = Status.Active.Code,
            Tags = ["users", "kullanıcılar"]
        };
        var response = await _appService.UpdateAsync(_testData.ResourceGroupId_1, input);
        response.ShouldNotBeNull();
        response.Tags?.Length.ShouldBeGreaterThanOrEqualTo(2);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnResourceGroupDto()
    {
        var response = await _appService.GetAsync(_testData.ResourceGroupId_1);

        response.ShouldNotBeNull();
        response.Id.ShouldBe(_testData.ResourceGroupId_1);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnPagedResult()
    {
        var pagedInput = new PagedResourceGroupInput()
        {
            SkipCount = 0,
            MaxResultCount = 1
        };
        var result = await _appService.GetAllAsync(pagedInput);
        result.ShouldNotBeNull();
        result.Items.ShouldHaveSingleItem();
    }
}
