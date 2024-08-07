using System;
using System.Linq;
using System.Threading.Tasks;
using BBT.Prism.Domain.EntityFrameworkCore;
using BBT.Resource.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BBT.Resource.Roles;

public class EfCoreRoleDefinitionRepository(ResourceDbContext dbContext, IServiceProvider serviceProvider)
    : EfCoreRepository<ResourceDbContext, RoleDefinition, Guid>(dbContext, serviceProvider), IRoleDefinitionRepository
{
    public override async Task<IQueryable<RoleDefinition>> WithDetailsAsync()
    {
        return (await GetQueryableAsync())
            .Include(p => p.Translations);
    }
}
