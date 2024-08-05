using System;
using BBT.Prism.Domain.Entities.Auditing;

namespace BBT.Resource.Roles;

public class RoleGroupRole : AuditedEntity
{
    public Guid GroupId { get; set; }
    public Guid RoleId { get; set; }
    public Status Status { get; set; }

    public override object?[] GetKeys()
    {
        return [GroupId, RoleId, Status];
    }
}
