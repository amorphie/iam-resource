using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BBT.Prism.Domain.Entities;
using BBT.Prism.Domain.EntityFrameworkCore;
using BBT.Resource.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BBT.Resource.Roles;

public class EfCoreRoleRepository(ResourceDbContext dbContext, IServiceProvider serviceProvider)
    : EfCoreRepository<ResourceDbContext, Role, Guid>(dbContext, serviceProvider), IRoleRepository
{
    public override async Task<IQueryable<Role>> WithDetailsAsync()
    {
        return (await GetQueryableAsync())
            .Include(p => p.Translations);
    }

    public async Task<RoleWithDefinitionModel> GetWithDefinitionAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var roleEntity = await (from role in dbContext.Roles.Include(i => i.Translations).AsNoTracking()
                where role.Id == id
                join definition in dbContext.RoleDefinitions.Include(i => i.Translations).AsNoTracking()
                    on role.DefinitionId equals definition.Id
                select new RoleWithDefinitionModel
                {
                    Role = role,
                    Definition = definition
                })
            .FirstOrDefaultAsync(cancellationToken);

        if (roleEntity == null)
        {
            throw new EntityNotFoundException(typeof(Role), id);
        }

        return roleEntity;
    }
}
