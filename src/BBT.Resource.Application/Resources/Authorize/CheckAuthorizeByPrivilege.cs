using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using BBT.Resource.Extensions;
using Elastic.Apm.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BBT.Resource.Resources.Authorize;

public class CheckAuthorizeByPrivilege(
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

        var resourcePrivileges = await resourceRepository.GetPrivilegesAsync(resource.Id, cancellationToken);

        if (!resourcePrivileges.Any())
        {
            if (AuthorizeOptions.AllowEmptyPrivilege)
            {
                return output.SetResult(403, "Allow empty privilege active.");
            }

            return output.SetResult(403, "Resource rules not found.");
        }

        var parameterList = new Dictionary<string, object>();

        SetRuleParameterList(resource, url, data, httpContext, parameterList);

        var transaction = Elastic.Apm.Agent.Tracer.CurrentTransaction ??
                          Elastic.Apm.Agent.Tracer.StartTransaction("CallApi", ApiConstants.TypeUnknown);

        foreach (ResourcePrivilegeModel resourcePrivilege in resourcePrivileges)
        {
            var privilegeUrl = resourcePrivilege.Privilege.Url;

            foreach (var variable in parameterList)
                privilegeUrl = privilegeUrl.Replace(variable.Key, variable.Value.ToString());

            var apiClientService = new HttpClientService();
            var span = transaction.StartSpan($"CallApi-{privilegeUrl}", ApiConstants.TypeUnknown);
            try
            {
                var headers = new Dictionary<string, object>();
                foreach (var header in httpContext.Request.Headers)
                {
                    headers.Add(header.Key, header.Value.ToString());
                }

                var response = await apiClientService.SendRequestAsync(
                    privilegeUrl,
                    string.IsNullOrEmpty(data) ? "" : data,
                    resourcePrivilege.Privilege.Type.ToString(),
                    headers,
                    span);

                if (!response.IsSuccessStatusCode)
                {
                    return output.SetResult(403, "FAILED");
                }
            }
            catch (Exception e)
            {
                span.CaptureException(e);
            }
            finally
            {
                span.End();
            }
        }

        return output.SetResult(200, "SUCCESS");
    }

    private void SetRuleParameterList(
        Resource resource,
        string url,
        string? data,
        HttpContext httpContext,
        Dictionary<string, object> ruleParams
    )
    {
        foreach (var header in httpContext.Request.Headers)
        {
            ruleParams.Add($"{{header.{header.Key.ToClean()}}}", header.Value);
        }

        var match = Regex.Match(url, resource.Url);
        if (match.Success)
        {
            foreach (Group pathVariable in match.Groups)
            {
                ruleParams.Add($"{{path.var{pathVariable.Name}}}", pathVariable.Value);
            }
        }

        var bodyParamList = new Dictionary<string, object>();
        if (!string.IsNullOrEmpty(data))
        {
            var jsonObject = JsonConvert.DeserializeObject<JObject>(data);
            if (jsonObject != null) bodyParamList = AuthorizeHelper.ToDictionary(jsonObject);
        }

        foreach (var body in bodyParamList)
        {
            ruleParams.Add($"{body}.{body.Key}", body.Value);
        }
    }
}
