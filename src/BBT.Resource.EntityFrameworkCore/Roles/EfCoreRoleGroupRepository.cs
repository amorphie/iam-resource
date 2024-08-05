using System;
using BBT.Prism.Domain.EntityFrameworkCore;
using BBT.Resource.EntityFrameworkCore;

namespace BBT.Resource.Roles;

public class EfCoreRoleGroupRepository(ResourceDbContext dbContext, IServiceProvider serviceProvider)
    : EfCoreRepository<ResourceDbContext, RoleGroup, Guid>(dbContext, serviceProvider), IRoleGroupRepository;
