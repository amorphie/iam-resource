using System;
using System.Threading.Tasks;
using BBT.Prism.Domain.Repositories;
using BBT.Prism.Modularity;
using BBT.Prism.Uow;
using Shouldly;
using Xunit;

namespace BBT.Resource.Permissions;

public abstract class PermissionManagerTests<TStartupModule> : ResourceDomainTestBase<TStartupModule>
    where TStartupModule : IPrismModule
{
    private readonly IPermissionManager _permissionManager;
    private readonly IPermissionGrantRepository _permissionGrantRepository;
    private readonly TestData _testData;
    private readonly IUnitOfWork _unitOfWork;

    protected PermissionManagerTests()
    {
        _permissionManager = GetRequiredService<IPermissionManager>();
        _permissionGrantRepository = GetRequiredService<IPermissionGrantRepository>();
        _testData = GetRequiredService<TestData>();
        _unitOfWork = GetRequiredService<IUnitOfWork>();
    }

    [Fact]
    public async Task DenyAsync_Should_Call_DeleteAsync()
    {
        // Arrange
        var permission = await _permissionGrantRepository.FindAsync(
            _testData.ApplicationId,
            _testData.ClientId,
            _testData.ProviderName,
            _testData.ProviderKey,
            _testData.PermissionName_1);

        // Act
        await _permissionManager.DenyAsync(permission!);
        await _unitOfWork.SaveChangesAsync();
        // Assert
        var existsPermissionGrant = await _permissionGrantRepository.FirstOrDefaultAsync(p => p.Id == permission.Id);
        existsPermissionGrant.ShouldBeNull();
    }

    [Fact]
    public async Task GrantAsync_Should_Call_InsertAsync()
    {
        // Arrange
        var permission = new PermissionGrant(
            new Guid(),
            _testData.ApplicationId,
            _testData.ClientId,
            _testData.PermissionName_6,
            _testData.ProviderName,
            _testData.ProviderKey);

        // Act
        await _permissionManager.GrantAsync(permission);
        await _unitOfWork.SaveChangesAsync();
        // Assert
        var existsPermissionGrant = await _permissionGrantRepository.FindAsync(
            _testData.ApplicationId,
            _testData.ClientId,
            _testData.ProviderName,
            _testData.ProviderKey,
            _testData.PermissionName_6
        );

        existsPermissionGrant.ShouldNotBeNull();
    }

    [Fact]
    public async Task CheckAsync_Should_Return_Cached_Result_If_Exists()
    {
        // Act
        var result = await _permissionManager.CheckAsync(
            _testData.ApplicationId,
            _testData.ClientId,
            _testData.ProviderName,
            _testData.ProviderKey,
            _testData.PermissionName_1);

        // Assert
        result.ShouldBeTrue();
    }
}
