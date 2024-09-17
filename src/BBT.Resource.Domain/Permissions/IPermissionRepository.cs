using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BBT.Prism.Domain.Repositories;

namespace BBT.Resource.Permissions;

public interface IPermissionRepository : IRepository<Permission, Guid>
{
    Task<List<Permission>> GetListAsync(string applicationId, string clientId, CancellationToken cancellationToken = default);
}
