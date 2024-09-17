using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BBT.Prism.Mapper;
using BBT.Resource.Extensions;
using BBT.Resource.Policies;
using BBT.Resource.PolicyEngine;
using BBT.Resource.Rules;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BBT.Resource.Resources.Authorize;

public class CheckAuthorizeByPolicy(
    IHttpContextAccessor httpContextAccessor,
    IResourceRepository resourceRepository,
    IPolicyEngine policyEngine,
    IOptions<CheckAuthorizeOptions> authorizeOptions,
    IObjectMapper mapper,
    IRuleRepository ruleRepository,
    PolicyMergeManager policyMergeManager,
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

        var mergedPolicies = new List<PolicyDefinition>();
        foreach (var resourcePolicy in resourcePolicies)
        {
            var mergedPolicy = await policyMergeManager.GetMergedPolicyAsync(
                mapper.Map<Policy, PolicyDefinition>(resourcePolicy.Policy),
                cancellationToken);
            mergedPolicies.Add(mergedPolicy);
        }

        var policyDefinitions = mergedPolicies
            .OrderBy(o => o.Priority)
            .Select(s => (IPolicy)s)
            .ToList();

        await BindRulesAsync(policyDefinitions, resourcePolicies, cancellationToken);

        var userContext = UserRequestContextBinder.Bind(
            httpContext: httpContext,
            urlPattern: resource.Url,
            url: url,
            data: data);

        var result =
            await policyEngine.EvaluatePoliciesAsync(
                policyDefinitions,
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

    private async Task BindRulesAsync(
        List<IPolicy> policyDefinitions,
        List<ResourcePolicyModel> resourcePolicies,
        CancellationToken cancellationToken = default)
    {
        foreach (var policyDefinition in policyDefinitions)
        {
            var policy = resourcePolicies.FirstOrDefault(p => p.Policy.Id.ToString() == policyDefinition.Id);
            if (policy != null)
            {
                var ruleIds = policy.Policy.Condition.Rules
                    .Select(Guid.Parse)
                    .ToArray();

                var rules = await ruleRepository.GetByIdsAsync(ruleIds, cancellationToken);
                policyDefinition.Conditions.Rules = rules.Select(s => new RuleCondition(s.Name, s.Expression)).ToList();
            }
        }
    }
}
