using System.Threading;
using System.Threading.Tasks;
using BBT.Prism.Application.Services;

namespace BBT.Resource.Permissions;

public interface IPermissionAppService : IApplicationService
{
    Task<PermissionListResultDto> GetAsync(
        string applicationId,
        string clientId,
        string providerName,
        string providerKey,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        string applicationId,
        string clientId,
        string providerName,
        string providerKey,
        UpdatePermissionInput input,
        CancellationToken cancellationToken = default);

    Task<CheckPermissionResultDto> CheckAsync(
        string applicationId,
        string clientId,
        string providerName,
        string providerKey,
        string name,
        CancellationToken cancellationToken = default);
}
