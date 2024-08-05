using System;
using BBT.Prism.Domain.Entities.Auditing;

namespace BBT.Resource.Resources;

public class ResourceRule : AuditedEntity
{
    public Guid ResourceId { get; private set; }
    public Guid RuleId { get; private set; }
    public Guid? ClientId { get; private set; }
    public Status Status { get; private set; }
    public int Priority { get; private set; }

    private ResourceRule()
    {
        //For Orm
    }

    public ResourceRule(
        Guid resourceId,
        Guid ruleId,
        Guid? clientId,
        int priority)
    {
        ResourceId = resourceId;
        RuleId = ruleId;
        ClientId = clientId;
        Priority = priority;
        Status = Status.Active;
    }

    public void Active()
    {
        Status = Status.Active;
    }

    public void Passive()
    {
        Status = Status.Passive;
    }

    public override object?[] GetKeys()
    {
        return [ResourceId, RuleId, ClientId, Status];
    }
}
