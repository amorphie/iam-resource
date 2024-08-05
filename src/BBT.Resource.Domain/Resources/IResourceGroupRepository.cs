using System;
using BBT.Prism.Domain.Repositories;

namespace BBT.Resource.Resources;

public interface IResourceGroupRepository : IRepository<ResourceGroup, Guid>
{
}
