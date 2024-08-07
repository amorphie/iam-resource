using System;
using BBT.Prism.Auditing;
using BBT.Resource.Privileges;

namespace BBT.Resource.Resources;

public class ResourcePrivilegeDto: IHasCreatedAt, IHasModifyTime
{
    public Guid PrivilegeId { get; set; }
    public Guid? ClientId { get; set; }
    public Status Status { get; set; }
    public int Priority { get; set; }
    public string PrivilegeUrl { get; set; }
    public PrivilegeType PrivilegeType { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
