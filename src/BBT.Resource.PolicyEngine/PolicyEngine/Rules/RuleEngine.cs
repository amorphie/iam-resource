using System.Dynamic;
using System.Text;
using System.Text.RegularExpressions;
using BBT.Resource.PolicyEngine.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RulesEngine.Models;

namespace BBT.Resource.PolicyEngine.Rules;

internal sealed class RuleEngine(ILogger<PolicyEngineModule> logger) : IRuleEngine
{
    public async Task<List<RuleResultTree>> ExecuteRules(RuleContext ruleContext)
    {
        var ruleParams = new List<RuleParameter>();
        SetRuleParameterList(ruleContext, ruleParams);
        return await ExecuteRules(ruleParams, ruleContext.RuleDefinitions);
    }

    private void SetRuleParameterList(
        RuleContext ruleContext,
        List<RuleParameter> ruleParams
    )
    {
        dynamic header = new ExpandoObject();

        foreach (var requestHeader in ruleContext.AllHeaders)
        {
            ((IDictionary<string, object>)header).Add(requestHeader.Key.ToClean(), requestHeader.Value);
        }

        var ruleParamHeader = new RuleParameter("header", header);
        ruleParams.Add(ruleParamHeader);

        dynamic path = new ExpandoObject();
        var match = Regex.Match(ruleContext.RequestUrl, ruleContext.UrlPattern);
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

        if (!string.IsNullOrEmpty(ruleContext.Data))
        {
            var jsonObject = JsonConvert.DeserializeObject<JObject>(ruleContext.Data);
            if (jsonObject != null) bodyParamList = RuleHelper.ToDictionary(jsonObject);
        }

        dynamic bodyObject = RuleHelper.ToExpandoObject(bodyParamList);

        var ruleParamBody = new RuleParameter("body", bodyObject);

        ruleParams.Add(ruleParamBody);
    }

    private async ValueTask<List<RuleResultTree>> ExecuteRules(
        List<RuleParameter> ruleParameters,
        List<RuleDefinition> ruleDefinitions)
    {
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
