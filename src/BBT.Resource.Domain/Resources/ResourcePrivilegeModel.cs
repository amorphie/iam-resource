using BBT.Resource.Privileges;

namespace BBT.Resource.Resources;

public class ResourcePrivilegeModel
{
    public Privilege Privilege { get; set; }
    public ResourcePrivilege RelatedPrivilege { get; set; }
}
