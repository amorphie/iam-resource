using System;
using BBT.Prism.Domain.Entities.Auditing;

namespace BBT.Resource.Resources;

public class ResourcePrivilege : AuditedEntity
{
    public Guid ResourceId { get; set; }
    public Guid PrivilegeId { get; set; }
    public Guid? ClientId { get; set; }
    public Status Status { get; set; }
    public int Priority { get; set; }
    public override object?[] GetKeys()
    {
        return [ResourceId, PrivilegeId, ClientId, Status];
    }
}
