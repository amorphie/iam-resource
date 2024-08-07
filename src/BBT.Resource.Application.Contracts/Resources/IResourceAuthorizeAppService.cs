using System.Threading;
using System.Threading.Tasks;
using BBT.Prism.Application.Services;

namespace BBT.Resource.Resources;

public interface IResourceAuthorizeAppService : IApplicationService
{
    Task<CheckAuthorizeOutput> CheckAsync(CheckAuthorizeInput input, CancellationToken cancellationToken = default);
}
