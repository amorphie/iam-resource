using System;
using BBT.Prism.Domain.EntityFrameworkCore;
using BBT.Resource.EntityFrameworkCore;

namespace BBT.Resource.Resources;

public class EfCoreResourceRepository(ResourceDbContext dbContext, IServiceProvider serviceProvider)
    : EfCoreRepository<ResourceDbContext, Resource, Guid>(dbContext, serviceProvider), IResourceRepository;
