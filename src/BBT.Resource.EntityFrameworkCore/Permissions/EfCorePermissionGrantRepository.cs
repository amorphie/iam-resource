using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BBT.Prism.Domain.EntityFrameworkCore;
using BBT.Resource.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BBT.Resource.Permissions;

public class EfCorePermissionGrantRepository(ResourceDbContext dbContext, IServiceProvider serviceProvider)
    : EfCoreRepository<ResourceDbContext, PermissionGrant, Guid>(dbContext, serviceProvider), IPermissionGrantRepository
{
    public async Task<List<PermissionGrant>> GetListAsync(
        string applicationId,
        string clientId,
        string providerName,
        string providerKey,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(p =>
                p.ApplicationId == applicationId
                && p.ClientId == clientId
                && p.ProviderName == providerName
                && p.ProviderKey == providerKey)
            .ToListAsync(cancellationToken);
    }

    public async Task<PermissionGrant?> FindAsync(
        string applicationId,
        string clientId,
        string providerName,
        string providerKey,
        string name,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .FirstOrDefaultAsync(p =>
                    p.ApplicationId == applicationId
                    && p.ClientId == clientId
                    && p.ProviderName == providerName
                    && p.ProviderKey == providerKey
                    && p.Name == name,
                cancellationToken);
    }
}
