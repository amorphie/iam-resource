using System;
using BBT.Prism.Domain.Repositories;

namespace BBT.Resource.Privileges;

public interface IPrivilegeRepository : IRepository<Privilege, Guid>
{
}
