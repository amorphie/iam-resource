using Microsoft.AspNetCore.Http;

namespace BBT.Resource.PolicyEngine;

public class UserRequestContextBinder
{
    public static UserRequestContext Bind(
        HttpContext httpContext,
        string urlPattern,
        string? url,
        string? data)
    {
        var userContext = new UserRequestContext
        {
            UrlPattern = urlPattern,
            RequestUrl = url,
            Data = data,
            UserId = httpContext.Request.Headers.ContainsKey("userid")
                ? httpContext.Request.Headers["userid"].ToString()
                : string.Empty,
            AllHeaders = httpContext.Request.Headers
                .ToDictionary(h => h.Key, h => h.Value.ToString()),
            Roles = httpContext.Request.Headers.ContainsKey("role")
                ? httpContext.Request.Headers["role"].ToString().Split(',').ToList()
                : new List<string>(),
            Attributes = httpContext.Request.Headers
                .Where(h => h.Key.StartsWith("atr_"))
                .ToDictionary(h => h.Key.Replace("atr_", ""), h => h.Value.ToString()),
            Time = DateTime.UtcNow
        };

        return userContext;
    }
}
