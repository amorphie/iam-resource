using System;
using BBT.Prism;
using BBT.Prism.Domain.Entities;

namespace BBT.Resource.Permissions;

public class PermissionGrant : Entity<Guid>
{
    public string ApplicationId { get; private set; }
    public string ClientId { get; private set; }

    /// <summary>
    /// Permission Name <seealso cref="Permission"/>
    /// </summary>
    public string Name { get; private set; }
    public string ProviderName { get; private set; }
    public string ProviderKey { get; private set; }

    private PermissionGrant()
    {
        // For ORM        
    }

    public PermissionGrant(
        Guid id,
        string applicationId,
        string clientId,
        string name,
        string providerName,
        string providerKey)
        : base(id)
    {
        ApplicationId =
            Check.NotNullOrEmpty(applicationId, nameof(ApplicationId), PermissionsConsts.MaxApplicationIdLength);
        ClientId = Check.NotNullOrEmpty(clientId, nameof(ClientId), PermissionsConsts.MaxClientIdLength);
        Name = Check.NotNullOrEmpty(name, nameof(Name), PermissionsConsts.MaxNameLength);
        ProviderName =
            Check.NotNullOrEmpty(providerName, nameof(ProviderName), PermissionsConsts.MaxProviderNameLength);
        ProviderKey = Check.NotNullOrEmpty(providerKey, nameof(ProviderKey), PermissionsConsts.MaxProviderKeyLength);
    }
}
