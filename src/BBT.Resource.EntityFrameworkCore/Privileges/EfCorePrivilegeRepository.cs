using System;
using BBT.Prism.Domain.EntityFrameworkCore;
using BBT.Resource.EntityFrameworkCore;

namespace BBT.Resource.Privileges;

public class EfCorePrivilegeRepository(ResourceDbContext dbContext, IServiceProvider serviceProvider)
    : EfCoreRepository<ResourceDbContext, Privilege, Guid>(dbContext, serviceProvider), IPrivilegeRepository;
