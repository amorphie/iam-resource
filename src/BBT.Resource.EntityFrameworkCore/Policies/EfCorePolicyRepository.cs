using System;
using BBT.Prism.Domain.EntityFrameworkCore;
using BBT.Resource.EntityFrameworkCore;

namespace BBT.Resource.Policies;

public class EfCorePolicyRepository(ResourceDbContext dbContext, IServiceProvider serviceProvider)
    : EfCoreRepository<ResourceDbContext, Policy, Guid>(dbContext, serviceProvider), IPolicyRepository;
