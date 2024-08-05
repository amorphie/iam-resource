using System;
using BBT.Prism.Domain.Entities.Auditing;

namespace BBT.Resource.Resources;

public class ResourceRole : AuditedEntity
{
    public Guid ResourceId { get; private set; }
    public Guid RoleId { get; private set; }
    public Status Status { get; private set; }

    private ResourceRole()
    {
        //For Orm
    }

    internal ResourceRole(
        Guid resourceId, Guid roleId)
    {
        ResourceId = resourceId;
        RoleId = roleId;
        Status = Status.Active;
    }

    public override object?[] GetKeys()
    {
        return [ResourceId, RoleId, Status];
    }
}
