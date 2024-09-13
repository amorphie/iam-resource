using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BBT.Resource.Extensions;
using BBT.Resource.PolicyEngine;
using BBT.Resource.PolicyEngine.Rules;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BBT.Resource.Resources.Authorize;

public class CheckAuthorizeByPolicy(
    IHttpContextAccessor httpContextAccessor,
    IResourceRepository resourceRepository,
    IPolicyEngine policyEngine,
    IOptions<CheckAuthorizeOptions> authorizeOptions,
    ILogger<CheckAuthorizeByPolicy> logger) : ICheckAuthorize
{
    private CheckAuthorizeOptions AuthorizeOptions { get; } = authorizeOptions.Value;

    public async Task<AuthorizeOutput> CheckAsync(string url, string method, string? data,
        CancellationToken cancellationToken = default)
    {
        var output = new AuthorizeOutput();
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            return output.SetResult(200, "HttpContext not found.");
        }

        var resource = await resourceRepository.FindByRegexAsync(url, method.ToResourceType(), cancellationToken);
        if (resource == null)
        {
            return output.SetResult(200, "Resource not found.");
        }

        var resourcePolicies = await resourceRepository.GetPoliciesAsync(resource.Id, cancellationToken);

        if (!resourcePolicies.Any())
        {
            if (AuthorizeOptions.AllowEmptyPrivilege)
            {
                return output.SetResult(200, "Allow empty policy active.");
            }

            return output.SetResult(403, "Resource policy not found.");
        }

        var userContext = UserRequestContextBinder.Bind(
            httpContext: httpContext,
            urlPattern: resource.Url,
            url: url,
            data: data);

        var result =
            await policyEngine.EvaluatePoliciesAsync(
                resourcePolicies
                    .Select(s => (IPolicy)s)
                    .ToList(),
                userContext);
        // result.GetDetailedReport();
        if (result.IsAllowed)
        {
            output.SetResult(200, "SUCCESS");
        }
        else
        {
            output.SetResult(403, "FAILED");
        }

        return output;
    }
}
