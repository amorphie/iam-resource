using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BBT.Resource.Permissions;

public interface IPermissionGrantStrategy
{
    Task<List<PermissionGrantInfo>> GetAsync(
        string applicationId,
        string clientId,
        string providerName,
        string providerKey,
        CancellationToken cancellationToken = default);
}
