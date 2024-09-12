using System;
using System.Linq;
using System.Threading.Tasks;
using BBT.Prism.Domain.EntityFrameworkCore;
using BBT.Resource.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BBT.Resource.Policies;

public class EfCorePolicyRepository(ResourceDbContext dbContext, IServiceProvider serviceProvider)
    : EfCoreRepository<ResourceDbContext, Policy, Guid>(dbContext, serviceProvider), IPolicyRepository
{
    public override async Task<IQueryable<Policy>> WithDetailsAsync()
    {
        return (await GetQueryableAsync())
            .Include(p => p.Condition);
    }
}
