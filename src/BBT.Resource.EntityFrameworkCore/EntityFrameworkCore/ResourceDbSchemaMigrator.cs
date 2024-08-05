using System.Threading.Tasks;
using BBT.Resource.Data;
using Microsoft.EntityFrameworkCore;

namespace BBT.Resource.EntityFrameworkCore;

public class ResourceDbSchemaMigrator(ResourceDbContext dbContext) : IResourceDbSchemaMigrator
{
    public async Task MigrateAsync()
    {
        await dbContext
            .Database.MigrateAsync();
    }
}