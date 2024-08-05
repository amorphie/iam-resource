using System;
using BBT.Prism.Domain.Repositories;

namespace BBT.Resource.Resources;

public interface IResourceRepository : IRepository<Resource, Guid>
{
}
