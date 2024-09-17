using BBT.Prism.Modularity;

namespace BBT.Resource.Permissions;

public abstract class PermissionRepositoryTests<TStartupModule> : ResourceDomainTestBase<TStartupModule>
    where TStartupModule : IPrismModule;
