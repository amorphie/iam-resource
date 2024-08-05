using System;
using BBT.Prism.Domain.EntityFrameworkCore;
using BBT.Resource.EntityFrameworkCore;

namespace BBT.Resource.Resources;

public class EfCoreResourceGroupRepository(ResourceDbContext dbContext, IServiceProvider serviceProvider)
    : EfCoreRepository<ResourceDbContext, ResourceGroup, Guid>(dbContext, serviceProvider), IResourceGroupRepository;
