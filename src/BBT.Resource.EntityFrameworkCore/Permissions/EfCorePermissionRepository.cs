using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BBT.Prism.Domain.EntityFrameworkCore;
using BBT.Resource.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BBT.Resource.Permissions;

public class EfCorePermissionRepository(ResourceDbContext dbContext, IServiceProvider serviceProvider)
    : EfCoreRepository<ResourceDbContext, Permission, Guid>(dbContext, serviceProvider), IPermissionRepository
{
    public override async Task<IQueryable<Permission>> WithDetailsAsync()
    {
        return (await GetDbSetAsync())
            .Include(p => p.Translations);
    }

    public async Task<List<Permission>> GetListAsync(
        string applicationId,
        string clientId,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(p =>
                p.ApplicationId == applicationId
                && p.ClientId == clientId)
            .ToListAsync(cancellationToken);
    }
}
