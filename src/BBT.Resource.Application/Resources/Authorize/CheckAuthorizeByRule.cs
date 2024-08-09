using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using BBT.Resource.Extensions;
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
    ILogger<CheckAuthorizeByRule> logger) : ICheckAuthorize
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
                return output.SetResult(403, "Allow empty privilege active.");
            }

            return output.SetResult(403, "Resource rules not found.");
        }

        var ruleParams = new List<RuleParameter>();
        SetRuleParameterList(resource, url, data, httpContext, ruleParams);

        var resultList = await ExecuteRules(ruleParams, resourceRules);

        if (resultList.Any(t => t.IsSuccess == false))
        {
            return output.SetResult(403, "FAILED");
        }

        return output.SetResult(200, "SUCCESS");
    }

    private void SetRuleParameterList(
        Resource resource,
        string url,
        string? data,
        HttpContext httpContext,
        List<RuleParameter> ruleParams
    )
    {
        dynamic header = new ExpandoObject();

        foreach (var requestHeader in httpContext.Request.Headers)
        {
            ((IDictionary<string, object>)header).Add(requestHeader.Key.ToClean(), requestHeader.Value.ToString());
        }

        var ruleParamHeader = new RuleParameter("header", header);
        ruleParams.Add(ruleParamHeader);

        dynamic path = new ExpandoObject();
        var match = Regex.Match(url, resource.Url);
        if (match.Success)
        {
            foreach (Group pathVariable in match.Groups)
            {
                ((IDictionary<string, object>)path).Add($"var{pathVariable.Name}", pathVariable.Value);
            }
        }

        var ruleParamPath = new RuleParameter("path", path);
        ruleParams.Add(ruleParamPath);

        var bodyParamList = new Dictionary<string, object>();

        if (!string.IsNullOrEmpty(data))
        {
            var jsonObject = JsonConvert.DeserializeObject<JObject>(data);
            if (jsonObject != null) bodyParamList = AuthorizeHelper.ToDictionary(jsonObject);
        }

        dynamic bodyObject = AuthorizeHelper.ToExpandoObject(bodyParamList);

        var ruleParamBody = new RuleParameter("body", bodyObject);

        ruleParams.Add(ruleParamBody);
    }

    private async ValueTask<List<RuleResultTree>> ExecuteRules(
        List<RuleParameter> ruleParameters,
        List<ResourceRuleModel> resourceRules)
    {
        var ruleDefinitions = resourceRules
            .Select(s =>
                new RuleDefinition(s.Rule.Name, s.Rule.Expression)
            ).ToList();

        var workflowRuleDefinition = new WorkflowRuleDefinition(
            "workflow",
            ruleDefinitions.ToArray()
        );

        var workflowRules = new[] { JsonConvert.SerializeObject(workflowRuleDefinition) };

        var reSettings = new ReSettings
        {
            CustomTypes = new Type[] { typeof(Utils) },
        };

        var rulesEngine = new RulesEngine.RulesEngine(workflowRules, reSettings);

        var response = await rulesEngine.ExecuteAllRulesAsync(
            workflowRuleDefinition.WorkflowName,
            ruleParameters.ToArray()
        );

        var logRule = new StringBuilder();
        logRule.AppendLine("Execution Rules:");
        foreach (var responseItem in response)
        {
            if (responseItem.IsSuccess)
            {
                logRule.AppendLine($"- RuleName: {responseItem.Rule.RuleName}. Success: {responseItem.IsSuccess}");
            }
            else
            {
                logRule.AppendLine(
                    $"RuleName: {responseItem.Rule.RuleName}. Success: {responseItem.IsSuccess}. ExceptionMessage: {responseItem.ExceptionMessage}");
            }
        }

        logger.LogInformation(logRule.ToString());

        return response;
    }
}
