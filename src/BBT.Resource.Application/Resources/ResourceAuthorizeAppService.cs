using System;
using System.Threading;
using System.Threading.Tasks;
using BBT.Prism.Application.Services;

namespace BBT.Resource.Resources;

public class ResourceAuthorizeAppService(
    IServiceProvider serviceProvider,
    IResourceRepository resourceRepository)
    : ApplicationService(serviceProvider), IResourceAuthorizeAppService
{
    public Task<CheckAuthorizeOutput> CheckAsync(CheckAuthorizeInput input,
        CancellationToken cancellationToken = default)
    {
        
        throw new NotImplementedException();
    }
}
