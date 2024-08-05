using System.Threading.Tasks;
using BBT.Prism.Data.Seeding;

namespace BBT.Resource.Data;

public class ResourceDataSeedContributor()
    : IDataSeedContributor
{
    public Task SeedAsync(DataSeedContext context)
    {
        return Task.CompletedTask;
    }
}
