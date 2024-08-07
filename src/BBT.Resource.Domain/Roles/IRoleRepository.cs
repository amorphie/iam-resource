using System;
using System.Threading;
using System.Threading.Tasks;
using BBT.Prism.Domain.Repositories;

namespace BBT.Resource.Roles;

public interface IRoleRepository : IRepository<Role, Guid>
{
    Task<RoleWithDefinitionModel> GetWithDefinitionAsync(Guid id, CancellationToken cancellationToken = default);
}
