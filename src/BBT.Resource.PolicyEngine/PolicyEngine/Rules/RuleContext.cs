namespace BBT.Resource.PolicyEngine.Rules;

public class RuleContext
{
    public required string UrlPattern { get; set; }
    public required string RequestUrl { get; set; }
    public string? Data { get; set; }
    public Dictionary<string, string> AllHeaders { get; set; } = new();
    public List<RuleDefinition> RuleDefinitions { get; set; } = new();
}
