using System.Threading.Tasks;

namespace BBT.Resource.Data;

public interface IResourceDbSchemaMigrator
{
    Task MigrateAsync();
}