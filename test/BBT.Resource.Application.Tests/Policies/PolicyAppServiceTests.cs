using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BBT.Prism.Modularity;
using Shouldly;
using Xunit;

namespace BBT.Resource.Policies;

public abstract class PolicyAppServiceTests<TStartupModule> : ResourceApplicationTestBase<TStartupModule>
    where TStartupModule : IPrismModule
{
    private readonly IPolicyAppService _appService;
    private readonly TestData _testData;

    protected PolicyAppServiceTests()
    {
        _appService = GetRequiredService<IPolicyAppService>();
        _testData = GetRequiredService<TestData>();
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCreatedPolicy()
    {
        var input = new CreatePolicyInput()
        {
            Id = Guid.NewGuid(),
            Name = "Policy_99",
            Effect = "A",
            ConflictResolution = "N",
            Permissions = ["User.Create", "User.Delete"],
            Priority = 10,
            Condition = new PolicyConditionDto()
            {
                Attributes = new ObjectDictionary(new Dictionary<string, object?>()
                {
                    { "organization", "Burgan" },
                    { "group", "Kurumsal" }
                }),
                Context = new ObjectDictionary(new Dictionary<string, object?>()
                {
                    { "location", "HQ" },
                    { "device", "CorporateDevice" }
                }),
                Rules = [_testData.RuleId_1.ToString()],
                Roles = ["admin", "editor"],
                Time = new PolicyTimeDto()
                {
                    Start = new TimeOnly(9, 0),
                    End = new TimeOnly(11, 0),
                    Timezone = "UTC"
                }
            }
        };

        var response = await _appService.CreateAsync(input);
        response.ShouldNotBeNull();
        response.Id.ShouldBe(input.Id);
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedPolicy()
    {
        var input = new UpdatePolicyInput()
        {
            Name = "Policy_80",
            Effect = "A",
            ConflictResolution = "N",
            Permissions = ["User.Create", "User.Delete"],
            Priority = 10,
            Condition = new PolicyConditionDto()
            {
                Attributes = new ObjectDictionary(new Dictionary<string, object?>()
                {
                    { "organization", "Burgan" },
                    { "group", "Kurumsal" }
                }),
                Context = new ObjectDictionary(new Dictionary<string, object?>()
                {
                    { "location", "HQ" },
                    { "device", "CorporateDevice" }
                }),
                Rules = [_testData.RuleId_1.ToString(), _testData.RuleId_2.ToString()],
                Roles = ["admin", "editor"],
                Time = new PolicyTimeDto()
                {
                    Start = new TimeOnly(9, 0),
                    End = new TimeOnly(18, 0),
                    Timezone = "UTC"
                }
            }
        };
        var response = await _appService.UpdateAsync(_testData.PolicyId_1, input);
        response.ShouldNotBeNull();
        response.Name.ShouldBe(input.Name);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnPolicyDto()
    {
        var response = await _appService.GetAsync(_testData.PolicyId_1);

        response.ShouldNotBeNull();
        response.Id.ShouldBe(_testData.PolicyId_1);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnPagedResult()
    {
        var pagedInput = new PagedPolicyInput()
        {
            SkipCount = 0,
            MaxResultCount = 1
        };
        var result = await _appService.GetAllAsync(pagedInput);
        result.ShouldNotBeNull();
        result.Items.ShouldHaveSingleItem();
    }
}
