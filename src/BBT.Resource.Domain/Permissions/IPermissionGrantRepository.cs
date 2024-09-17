using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BBT.Prism.Domain.Repositories;

namespace BBT.Resource.Permissions;

public interface IPermissionGrantRepository : IRepository<PermissionGrant, Guid>
{
    Task<List<PermissionGrant>> GetListAsync(
        string applicationId,
        string clientId,
        string providerName,
        string providerKey,
        CancellationToken cancellationToken = default);
    
    Task<PermissionGrant?> FindAsync(
        string applicationId,
        string clientId,
        string providerName,
        string providerKey,
        string name,
        CancellationToken cancellationToken = default);
}
