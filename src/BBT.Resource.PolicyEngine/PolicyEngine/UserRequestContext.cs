namespace BBT.Resource.PolicyEngine;

public class UserRequestContext
{
    /// <summary>
    /// Resource Url
    /// </summary>
    public required string UrlPattern { get; set; }
    /// <summary>
    /// User Info
    /// IdentityNumber, Id etc.
    /// </summary>
    public string? UserId { get; set; }
    /// <summary>
    /// All headers in HttpContext
    /// </summary>
    public Dictionary<string, string>? AllHeaders { get; set; }
    /// <summary>
    /// Roles from header
    /// Key: role
    /// </summary>
    public List<string>? Roles { get; set; }
    /// <summary>
    /// Attributes from header
    /// Key prefix: atr_
    /// </summary>
    public Dictionary<string, string>? Attributes { get; set; }
    /// <summary>
    /// Context
    /// </summary>
    public Dictionary<string, string>? Context { get; set; }
    /// <summary>
    /// Request time
    /// </summary>
    public DateTime Time { get; set; }
    /// <summary>
    /// Body
    /// </summary>
    public string? Data { get; set; }
    /// <summary>
    /// Request Url
    /// </summary>
    public string? RequestUrl { get; set; }
    /// <summary>
    /// Additional data
    /// </summary>
    public Dictionary<string, string> AdditionalData { get; set; } = new();

    public string? FindApplicationId()
    {
        return FindValue("x-application-id");
    }
    
    internal string? FindClientId()
    {
        return FindValue("x-client-id");
    }

    internal string FindValue(string key)
    {
        if (AllHeaders == null)
        {
            return string.Empty;
        }

        if (AllHeaders.TryGetValue(key, out _))
        {
            return AllHeaders[key];
        }

        return string.Empty;
    }

    public string? FindProviderKeyByProvider(string provider)
    {
        if (provider == "U")
        {
            return UserId;
        }

        switch (provider)
        {
            case "R":
                return FindValue("role");
            case "C":
                return FindValue("client_id");
            default:
                return string.Empty;
        }
    } 
}
