using System;
using BBT.Prism.Domain.EntityFrameworkCore;
using BBT.Resource.EntityFrameworkCore;

namespace BBT.Resource.Roles;

public class EfCoreRoleDefinitionRepository(ResourceDbContext dbContext, IServiceProvider serviceProvider)
    : EfCoreRepository<ResourceDbContext, RoleDefinition, Guid>(dbContext, serviceProvider), IRoleDefinitionRepository;
