namespace BBT.Resource.PolicyEngine;

public class UserRequestContext
{
    /// <summary>
    /// Resource Url
    /// </summary>
    public string UrlPattern { get; set; }
    /// <summary>
    /// User Info
    /// IdentityNumber, Id etc.
    /// </summary>
    public string UserId { get; set; }
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
    public Dictionary<string, string>? Context { get; set; }
    /// <summary>
    /// Request time
    /// </summary>
    public DateTime Time { get; set; }
    public List<string>? Permissions { get; set; }
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
}
