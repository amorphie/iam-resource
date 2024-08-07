using System;
using BBT.Prism.Domain.Entities.Auditing;

namespace BBT.Resource.Resources;

public class ResourceRule : AuditedEntity
{
    public Guid ResourceId { get; private set; }
    public Guid RuleId { get; private set; }
    //TODO: ClientId is currently nullable, so it is not added to the index.
    public Guid? ClientId { get; private set; }
    public Status Status { get; private set; }
    public int Priority { get; private set; }

    private ResourceRule()
    {
        //For Orm
    }

    internal ResourceRule(
        Guid resourceId,
        Guid ruleId,
        Guid? clientId,
        int priority)
    {
        ResourceId = resourceId;
        RuleId = ruleId;
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

    public override object?[] GetKeys()
    {
        return [ResourceId, RuleId, ClientId];
    }
}
