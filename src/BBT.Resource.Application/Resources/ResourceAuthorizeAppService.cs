using System;
using System.Threading;
using System.Threading.Tasks;
using BBT.Prism.Application.Services;
using BBT.Resource.Resources.Authorize;

namespace BBT.Resource.Resources;

public class ResourceAuthorizeAppService(
    IServiceProvider serviceProvider,
    ICheckAuthorizeFactory checkAuthorizeFactory)
    : ApplicationService(serviceProvider), IResourceAuthorizeAppService
{
    public async Task<CheckAuthorizeOutput> CheckAsync(string? type, CheckAuthorizeInput input,
        CancellationToken cancellationToken = default)
    {
        CheckAuthorizeType checkType;
        if (type != null && type.Equals("Rule", StringComparison.OrdinalIgnoreCase))
        {
            checkType = CheckAuthorizeType.Rule;
        }
        else
        {
            checkType = CheckAuthorizeType.Privilege;
        }

        var checkAuthorize = checkAuthorizeFactory.Create(checkType);
        var result = await checkAuthorize.CheckAsync(
            input.Url,
            input.Method,
            input.Data,
            cancellationToken);

        return new CheckAuthorizeOutput(result.StatusCode, result.Reason);
    }
}
