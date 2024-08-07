using System;
using BBT.Prism.Domain.Entities.Auditing;

namespace BBT.Resource.Resources;

public class ResourcePrivilege : AuditedEntity
{
    public Guid ResourceId { get; set; }
    public Guid PrivilegeId { get; set; }
    //TODO: ClientId is currently nullable, so it is not added to the index.
    public Guid? ClientId { get; set; }
    public Status Status { get; set; }
    public int Priority { get; set; }

    public override object?[] GetKeys()
    {
        return [ResourceId, PrivilegeId, ClientId];
    }

    private ResourcePrivilege()
    {
        //For Orm
    }

    internal ResourcePrivilege(
        Guid resourceId,
        Guid privilegeId,
        Guid? clientId,
        int priority)
    {
        ResourceId = resourceId;
        PrivilegeId = privilegeId;
        ClientId = clientId;
        SetPriority(priority);
        Status = Status.Active;
    }

    internal void ChangeStatus(Status status)
    {
        Status = status;
    }

    internal void SetPriority(int priority = 1)
    {
        if (priority is > 0 and <= 10)
        {
            Priority = priority;
            return;
        }

        throw new ArgumentOutOfRangeException(
            nameof(Priority),
            "The value must be in the range of 1 to 10.");
    }
}
