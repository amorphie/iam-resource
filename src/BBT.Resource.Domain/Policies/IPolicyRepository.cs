using System;
using BBT.Prism.Domain.Repositories;

namespace BBT.Resource.Policies;

public interface IPolicyRepository : IRepository<Policy, Guid>
{
}
