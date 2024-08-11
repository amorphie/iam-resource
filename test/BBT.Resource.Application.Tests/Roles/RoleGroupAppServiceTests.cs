using System;
using System.Threading.Tasks;
using BBT.Prism.Modularity;
using Shouldly;
using Xunit;

namespace BBT.Resource.Roles;

public abstract class RoleGroupAppServiceTests<TStartupModule> : ResourceApplicationTestBase<TStartupModule>
    where TStartupModule : IPrismModule
{
    private readonly IRoleGroupAppService _appService;
    private readonly TestData _testData;

    protected RoleGroupAppServiceTests()
    {
        _appService = GetRequiredService<IRoleGroupAppService>();
        _testData = GetRequiredService<TestData>();
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCreatedRoleGroup()
    {
        var input = new CreateRoleGroupInput([new() { Language = "en-US", Name = "Accounting" }])
        {
            Id = Guid.NewGuid(),
            Tags = new[] { "accounting" }
        };

        var response = await _appService.CreateAsync(input);
        response.ShouldNotBeNull();
        response.Id.ShouldBe(input.Id);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedRoleGroup()
    {
        var input = new UpdateRoleGroupInput([
            new() { Language = "en-US", Name = "Internet Banking" }
        ])
        {
            Status = Status.Active.Code,
            Tags = ["internet-banking", "ib"]
        };
        var response = await _appService.UpdateAsync(_testData.RoleGroupId_1, input);
        response.ShouldNotBeNull();
        response.Translations.ShouldHaveSingleItem();
    }

    [Fact]
    public async Task GetAsync_ShouldReturnRoleGroupDto()
    {
        var response = await _appService.GetAsync(_testData.RoleGroupId_1);

        response.ShouldNotBeNull();
        response.Id.ShouldBe(_testData.RoleGroupId_1);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnPagedResult()
    {
        var pagedInput = new PagedRoleGroupInput()
        {
            SkipCount = 0,
            MaxResultCount = 1
        };
        var result = await _appService.GetAllAsync(pagedInput);
        result.ShouldNotBeNull();
        result.Items.ShouldHaveSingleItem();
    }

    [Fact]
    public async Task GetRolesAsync_ShouldReturnRoleGroupRoleDto()
    {
        var response = await _appService.GetRolesAsync(_testData.RoleGroupId_1);
        response.Items.Count.ShouldBeGreaterThanOrEqualTo(1);
    }

    [Fact]
    public async Task AddRoleAsync_ShouldAddRole()
    {
        var input = new AddRoleToRoleGroupInput()
        {
            RoleId = _testData.RoleId_1
        };

        await _appService.AddRoleAsync(_testData.RoleGroupId_2, input);
    }

    [Fact]
    public async Task AddRoleAsync_ShouldAddRole_IfRoleExists()
    {
        var input = new AddRoleToRoleGroupInput()
        {
            RoleId = _testData.RoleId_1
        };

        await _appService.AddRoleAsync(_testData.RoleGroupId_1, input);
    }
    
    [Fact]
    public async Task ChangeRoleStatusAsync_ShouldUpdateRoleStatus()
    {
        var input = new ChangeStatusOfGroupRoleInput()
        {
            Status = Status.Passive.Code
        };

        await _appService.ChangeRoleStatusAsync(_testData.RoleGroupId_1, _testData.RoleId_1, input);
    }

    [Fact]
    public async Task RemoveRoleAsync_ShouldRemoveRole()
    {
        await _appService.RemoveRoleAsync(_testData.RoleGroupId_1, _testData.RoleId_1);
    }
}
