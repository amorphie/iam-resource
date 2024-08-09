namespace BBT.Resource.Resources.Authorize;

public class RuleDefinition(string name, string expression)
{
    public string Name { get; } = name;
    public string Expression { get; } = expression;
}
