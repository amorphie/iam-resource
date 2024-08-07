using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BBT.Prism.Domain.Repositories;

namespace BBT.Resource.Roles;

public interface IRoleGroupRepository : IRepository<RoleGroup, Guid>
{
    Task<RoleGroupRelatedRoleModel> GetRoleAsync(Guid groupId, Guid roleId, CancellationToken cancellationToken = default);
    Task<List<RoleGroupRelatedRoleModel>> GetRolesAsync(Guid groupId, CancellationToken cancellationToken = default);
}
