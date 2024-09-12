using System;
using BBT.Prism.Domain.Entities.Auditing;

namespace BBT.Resource.Resources;

public class ResourcePolicy : AuditedEntity
{
    public Guid ResourceId { get; private set; }
    public Guid PolicyId { get; private set; }

    //TODO: overide policy metası ekle. 
    
    
    /// <summary>
    /// Client Ids
    /// </summary>
    public string[] Clients { get; set; }

    public Status Status { get; private set; }
    public int Priority { get; private set; }

    public override object?[] GetKeys()
    {
        return [ResourceId, PolicyId];
    }

    private ResourcePolicy()
    {
        //For ORM
    }

    public ResourcePolicy(
        Guid resourceId,
        Guid policyId,
        string[] clients,
        int priority)
    {
        ResourceId = resourceId;
        PolicyId = policyId;
        Clients = clients;
        SetPriority(priority);
        Status = Status.Active;
    }

    internal void ChangeStatus(Status status)
    {
        Status = status;
    }
    
    internal void ChangeClients(string[] clients)
    {
        Clients = clients;
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
