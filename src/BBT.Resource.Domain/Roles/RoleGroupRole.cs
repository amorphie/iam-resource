using System;
using BBT.Prism.Domain.Entities.Auditing;

namespace BBT.Resource.Roles;

public class RoleGroupRole : AuditedEntity
{
    public Guid GroupId { get; private set; }
    public Guid RoleId { get; private set; }
    public Status Status { get; private set; }

    public override object?[] GetKeys()
    {
        return [GroupId, RoleId];
    }

    private RoleGroupRole()
    {
        //For Orm
    }

    internal RoleGroupRole(
        Guid groupId,
        Guid roleId)
    {
        GroupId = groupId;
        RoleId = roleId;
        Status = Status.Active;
    }

    internal void ChangeStatus(Status status)
    {
        Status = status;
    }
}
