using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BBT.Prism.Auditing;
using BBT.Prism.Domain.Entities;
using BBT.Prism.MultiLingualEntities;

namespace BBT.Resource.Resources;

public class Resource : AggregateRoot<Guid>, IHasCreatedAt, IHasModifyTime, IMultiLingualEntity<ResourceTranslation>
{
    public Guid? GroupId { get; private set; }
    public ResourceType Type { get; private set; }
    public string Url { get; private set; }
    public string[]? Tags { get; set; }
    public Status Status { get; private set; }
    public DateTime CreatedAt { get; init; }
    public DateTime? ModifiedAt { get; init; }
    public ICollection<ResourceTranslation> Translations { get; set; }
    public ICollection<ResourceRule> Rules { get; private set; }
    public ICollection<ResourceRole> Roles { get; private set; }
    public ICollection<ResourcePrivilege> Privileges { get; private set; }

    private Resource()
    {
        //For ORM
    }

    public Resource(
        Guid id,
        Guid? groupId,
        ResourceType type,
        string url) : base(id)
    {
        GroupId = groupId;
        Type = type;
        Url = url;
        Status = Status.Active;
        Rules = new Collection<ResourceRule>();
        Roles = new Collection<ResourceRole>();
        Privileges = new Collection<ResourcePrivilege>();
        Translations = new Collection<ResourceTranslation>();
    }

    public void Active()
    {
        Status = Status.Active;
    }

    public void Passive()
    {
        Status = Status.Passive;
    }

    public void AddRule(Guid ruleId)
    {
    }

    public void RemoveRule()
    {
    }

    public void AddRole(Guid roleId)
    {
        Roles.Add(new ResourceRole(roleId, Id));
    }

    public void RemoveRole(ResourceRole resourceRole)
    {
        Roles.Remove(resourceRole);
    }

    public void AddPrivilege()
    {
    }

    public void RemovePrivilege()
    {
    }
}
