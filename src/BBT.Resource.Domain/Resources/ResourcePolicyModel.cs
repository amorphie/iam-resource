using BBT.Resource.Policies;

namespace BBT.Resource.Resources;

public class ResourcePolicyModel
{
    public Policy Policy { get; set; }
    public ResourcePolicy RelatedPolicy { get; set; }
}
