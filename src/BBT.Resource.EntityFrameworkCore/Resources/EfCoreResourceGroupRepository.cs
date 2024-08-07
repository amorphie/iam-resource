using System;
using System.Linq;
using System.Threading.Tasks;
using BBT.Prism.Domain.EntityFrameworkCore;
using BBT.Resource.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BBT.Resource.Resources;

public class EfCoreResourceGroupRepository(ResourceDbContext dbContext, IServiceProvider serviceProvider)
    : EfCoreRepository<ResourceDbContext, ResourceGroup, Guid>(dbContext, serviceProvider), IResourceGroupRepository
{
    public override async Task<IQueryable<ResourceGroup>> WithDetailsAsync()
    {
        return (await GetQueryableAsync())
            .Include(p => p.Translations);
    }
}
