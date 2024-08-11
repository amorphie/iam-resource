using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BBT.Prism.Domain.Entities;
using BBT.Prism.Domain.EntityFrameworkCore;
using BBT.Resource.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BBT.Resource.Roles;

public class EfCoreRoleGroupRepository(ResourceDbContext dbContext, IServiceProvider serviceProvider)
    : EfCoreRepository<ResourceDbContext, RoleGroup, Guid>(dbContext, serviceProvider), IRoleGroupRepository
{
    public override async Task<IQueryable<RoleGroup>> WithDetailsAsync()
    {
        return (await GetQueryableAsync())
            .Include(p => p.Translations)
            .Include(p => p.Roles);
    }

    public async Task<RoleGroupRelatedRoleModel> GetRoleAsync(Guid groupId, Guid roleId,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var entity = await (from related in dbContext.RoleGroupRoles.AsNoTracking()
                where related.GroupId == groupId && related.RoleId == roleId
                join role in dbContext.Roles.Include(r => r.Translations).AsNoTracking()
                    on related.RoleId equals role.Id
                select new RoleGroupRelatedRoleModel
                {
                    Role = role,
                    RelatedRole = related
                })
            .FirstOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            throw new EntityNotFoundException(typeof(RoleGroupRole), roleId);
        }

        return entity;
    }

    public async Task<List<RoleGroupRelatedRoleModel>> GetRolesAsync(Guid groupId,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        return await (from related in dbContext.RoleGroupRoles.AsNoTracking()
                where related.GroupId == groupId
                join role in dbContext.Roles.Include(i => i.Translations).AsNoTracking()
                    on related.RoleId equals role.Id
                select new RoleGroupRelatedRoleModel
                {
                    Role = role,
                    RelatedRole = related
                })
            .ToListAsync(cancellationToken);
    }
}
