using BBT.Prism.Modularity;
using BBT.Resource.Permissions.GrantProviders;
using Shouldly;
using Xunit;

namespace BBT.Resource.Permissions;

public abstract class PermissionGrantStrategyFactoryTests<TStartupModule> : ResourceDomainTestBase<TStartupModule>
    where TStartupModule : IPrismModule
{
    private readonly PermissionGrantStrategyFactory _factory;

    protected PermissionGrantStrategyFactoryTests()
    {
        _factory = GetRequiredService<PermissionGrantStrategyFactory>();
    }

    [Fact]
    public void CreateStrategy_Should_Return_UserPermissionGrantStrategy_When_ProviderName_Is_U()
    {
        // Act
        var strategy = _factory.CreateStrategy("U");

        // Assert
        strategy.ShouldBeOfType<UserPermissionGrantStrategy>();
    }

    [Fact]
    public void CreateStrategy_Should_Return_DefaultPermissionGrantStrategy_When_ProviderName_Is_Unknown()
    {
        // Act
        var strategy = _factory.CreateStrategy("Unknown");

        // Assert
        strategy.ShouldBeOfType<DefaultPermissionGrantStrategy>();
    }
}
