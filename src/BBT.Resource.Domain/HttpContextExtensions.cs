using Microsoft.AspNetCore.Http;

namespace BBT.Resource;

public static class HttpContextExtensions
{
    public static string ToRoles(this HttpContext httpContext)
    {
        httpContext.Request.Headers.TryGetValue("role", out var role);
        return role.ToString();
    }
    
    public static string GetClientId(this HttpContext httpContext)
    {
        httpContext.Request.Headers.TryGetValue("client_id", out var clientId);
        return clientId.ToString();
    }
}
