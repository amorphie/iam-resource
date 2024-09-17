using System.Linq;
using System.Threading.Tasks;
using BBT.Prism.Modularity;
using Xunit;

namespace BBT.Resource.Permissions;

public abstract class PermissionAppServiceTests<TStartupModule> : ResourceApplicationTestBase<TStartupModule>
    where TStartupModule : IPrismModule
{
    private readonly IPermissionAppService _appService;
    private readonly TestData _testData;

    protected PermissionAppServiceTests()
    {
        _appService = GetRequiredService<IPermissionAppService>();
        _testData = GetRequiredService<TestData>();
    }

    [Fact]
    public async Task GetAsync_Should_Return_Permissions_With_Granted_Status()
    {
        // Act
        var result = await _appService.GetAsync(
            _testData.ApplicationId,
            _testData.ClientId,
            _testData.ProviderName,
            _testData.ProviderKey);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.Permissions);
        var permission = result.Permissions.FirstOrDefault(p => p.Name == _testData.PermissionName_3);
        Assert.NotNull(permission);
        Assert.Equal("Identities.Users.Create", permission.Name);
        Assert.True(permission.IsGranted);
    }

    [Fact]
    public async Task UpdateAsync_Should_Grant_And_Revoke_Permissions_Correctly()
    {
        var updateInput = new UpdatePermissionInput
        {
            Permissions = new[]
            {
                new UpdatePermissionDto
                    { Name = _testData.PermissionName_3, IsGranted = true }, // Grant "Create" permission
                new UpdatePermissionDto
                    { Name = _testData.PermissionName_4, IsGranted = false }, // Revoke "Update" permission
                new UpdatePermissionDto
                    { Name = _testData.PermissionName_6, IsGranted = true } // Grant "Assign Roles" permission
            }
        };

        // Act
        await _appService.UpdateAsync(
            _testData.ApplicationId,
            _testData.ClientId,
            _testData.ProviderName,
            _testData.ProviderKey,
            updateInput);

        // Assert
        var result = await _appService.GetAsync(
            _testData.ApplicationId,
            _testData.ClientId,
            _testData.ProviderName,
            _testData.ProviderKey);
        Assert.NotNull(result);

        var grantedCreatePermission = result.Permissions.FirstOrDefault(p => p.Name == _testData.PermissionName_3);
        Assert.NotNull(grantedCreatePermission);
        Assert.True(grantedCreatePermission.IsGranted);

        var revokedUpdatePermission = result.Permissions.FirstOrDefault(p => p.Name == _testData.PermissionName_4);
        Assert.NotNull(revokedUpdatePermission);
        Assert.False(revokedUpdatePermission.IsGranted);

        var grantedAssignRolesPermission = result.Permissions.FirstOrDefault(p => p.Name == _testData.PermissionName_6);
        Assert.NotNull(grantedAssignRolesPermission);
        Assert.True(grantedAssignRolesPermission.IsGranted);
    }

    [Fact]
    public async Task CheckAsync_Should_Return_Permissions_With_Granted_Status()
    {
        // Act
        var result = await _appService.CheckAsync(
            _testData.ApplicationId,
            _testData.ClientId,
            _testData.ProviderName,
            _testData.ProviderKey,
            _testData.PermissionName_1);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsGranted);
    }
}
