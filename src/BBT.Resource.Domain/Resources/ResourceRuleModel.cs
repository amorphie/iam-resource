using BBT.Resource.Rules;

namespace BBT.Resource.Resources;

public class ResourceRuleModel
{
    public Rule Rule { get; set; }
    public ResourceRule RelatedRule { get; set; }
}
