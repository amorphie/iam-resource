using System;
using BBT.Prism.Modularity;
using BBT.Resource.Permissions.CheckProviders;
using Shouldly;
using Xunit;

namespace BBT.Resource.Permissions;

public abstract class PermissionCheckStrategyFactoryTests<TStartupModule> : ResourceDomainTestBase<TStartupModule>
    where TStartupModule : IPrismModule
{
    private readonly PermissionCheckStrategyFactory _factory;

    protected PermissionCheckStrategyFactoryTests()
    {
        _factory = GetRequiredService<PermissionCheckStrategyFactory>();
    }

    [Fact]
    public void CreateStrategy_Should_Return_UserPermissionCheckStrategy_When_ProviderName_Is_U()
    {
        // Act
        var strategy = _factory.CreateStrategy("U");

        // Assert
        strategy.ShouldBeOfType<UserPermissionCheckStrategy>();
    }

    [Fact]
    public void CreateStrategy_Should_Return_RolePermissionCheckStrategy_When_ProviderName_Is_R()
    {
        // Act
        var strategy = _factory.CreateStrategy("R");

        // Assert
        strategy.ShouldBeOfType<RolePermissionCheckStrategy>();
    }

    [Fact]
    public void CreateStrategy_Should_Throw_NotSupportedException_When_ProviderName_Is_Invalid()
    {
        // Act & Assert
        Should.Throw<NotSupportedException>(() => _factory.CreateStrategy("X"));
    }
}
