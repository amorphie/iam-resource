using System.Threading.Tasks;
using BBT.Prism.Data.Seeding;

namespace BBT.Resource;

public class ResourceTestDataSeedContributor: IDataSeedContributor
{
    public Task SeedAsync(DataSeedContext context)
    {
        /* Seed additional test data... */

        return Task.CompletedTask;
    }
}