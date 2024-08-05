using System;
using BBT.Prism.Domain.Repositories;

namespace BBT.Resource.Roles;

public interface IRoleGroupRepository : IRepository<RoleGroup, Guid>
{
}
