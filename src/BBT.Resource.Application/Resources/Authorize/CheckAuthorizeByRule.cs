using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using BBT.Resource.Extensions;
using BBT.Resource.PolicyEngine.Rules;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RulesEngine.Models;

namespace BBT.Resource.Resources.Authorize;

public class CheckAuthorizeByRule(
    IHttpContextAccessor httpContextAccessor,
    IResourceRepository resourceRepository,
    IOptions<CheckAuthorizeOptions> authorizeOptions,
    IRuleEngine ruleEngine) : ICheckAuthorize
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

        var resourceRules = await resourceRepository.GetRulesAsync(resource.Id, cancellationToken);

        if (!resourceRules.Any())
        {
            if (AuthorizeOptions.AllowEmptyPrivilege)
            {
                return output.SetResult(200, "Allow empty privilege active.");
            }

            return output.SetResult(403, "Resource rules not found.");
        }

        var resultList = await ruleEngine.ExecuteRules(new RuleContext()
        {
            AllHeaders = httpContext.Request.Headers
                .ToDictionary(
                    h => h.Key,
                    h => h.Value.ToString()
                ),
            Data = data,
            RequestUrl = url,
            UrlPattern = resource.Url,
            RuleDefinitions = resourceRules
                .Select(s =>
                    new RuleDefinition(
                        s.Rule.Name,
                        s.Rule.Expression)
                ).ToList()
        });

        if (resultList.Any(t => t.IsSuccess == false))
        {
            return output.SetResult(403, "FAILED");
        }

        return output.SetResult(200, "SUCCESS");
    }
}
