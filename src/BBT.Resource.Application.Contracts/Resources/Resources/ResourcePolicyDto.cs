using System;
using BBT.Prism.Auditing;
using BBT.Resource.Policies;

namespace BBT.Resource.Resources;

public class ResourcePolicyDto: IHasCreatedAt, IHasModifyTime
{
    public string PolicyName { get; set; }
    public Effect Effect { get; set; }
    public string[] Clients { get; set; }
    public Status Status { get; set; }
    public int Priority { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
