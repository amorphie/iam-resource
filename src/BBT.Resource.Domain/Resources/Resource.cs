using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BBT.Prism;
using BBT.Prism.Auditing;
using BBT.Prism.Domain.Entities;
using BBT.Prism.Domain.Entities.Auditing;
using BBT.Prism.MultiLingualEntities;

namespace BBT.Resource.Resources;

public class Resource : AuditedAggregateRoot<Guid>, IMultiLingualEntity<ResourceTranslation>
{
    public Guid? GroupId { get; private set; }
    public ResourceType Type { get; set; }
    public string Url { get; private set; }
    public string[]? Tags { get; set; }
    public Status Status { get; private set; }
    public ICollection<ResourceTranslation> Translations { get; set; }
    public ICollection<ResourceRule> Rules { get; private set; }
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
        SetUrl(url);
        Status = Status.Active;
        Rules = new Collection<ResourceRule>();
        Privileges = new Collection<ResourcePrivilege>();
        Translations = new Collection<ResourceTranslation>();
    }

    public void SetUrl(string url)
    {
        Url = Check.NotNullOrEmpty(url, nameof(Url), ResourceConsts.MaxUrlLength);
    }

    public void ChangeStatus(string status)
    {
        Status = Status.FromCode(status);
    }

    public void AddTranslation(string language, string name, string? description)
    {
        if (Translations.All(a => a.Language != language))
        {
            Translations.Add(new ResourceTranslation(Id, language, name, description));
        }
        else
        {
            var translation = Translations.First(p => p.Language == language);
            translation.SetName(name);
            translation.SetDescription(description);
        }
    }

    public void RemoveTranslation(ResourceTranslation translation)
    {
        Translations.Remove(translation);
    }

    public void AddRule(Guid ruleId, Guid? clientId = null, int priority = 1)
    {
        if (Rules.All(a => a.RuleId != ruleId))
        {
            Rules.Add(new ResourceRule(Id, ruleId, clientId, priority));
        }
        else
        {
            var rule = Rules.First(p => p.RuleId == ruleId);
            rule.ChangeStatus(Status.Active);
        }
    }

    public void RemoveRule(Guid ruleId)
    {
        if (Rules.Any(a => a.RuleId == ruleId))
        {
            Rules.Remove(Rules.First(p => p.RuleId == ruleId));
        }
    }

    public void UpdateRule(Guid ruleId, string status, int priority = 1)
    {
        if (Rules.Any(a => a.RuleId == ruleId))
        {
            var rule = Rules.First(p => p.RuleId == ruleId);
            rule.ChangeStatus(Status.FromCode(status));
            rule.SetPriority(priority);
        }
    }

    public void AddPrivilege(Guid privilegeId, Guid? clientId, int priority)
    {
        if (!Privileges.Any(a => a.PrivilegeId == privilegeId && a.Status.Equals(Status.Active)))
        {
            Privileges.Add(new ResourcePrivilege(Id, privilegeId, clientId, priority));
        }
        else
        {
            var rule = Privileges.First(p => p.PrivilegeId == privilegeId);
            rule.ChangeStatus(Status.Active);
        }
    }

    public void RemovePrivilege(Guid privilegeId)
    {
        if (Privileges.Any(a => a.PrivilegeId == privilegeId))
        {
            Privileges.Remove(Privileges.First(p => p.PrivilegeId == privilegeId));
        }
    }

    public void UpdatePrivilege(Guid privilegeId, string status, int priority = 1)
    {
        if (Privileges.Any(a => a.PrivilegeId == privilegeId))
        {
            var privilege = Privileges.First(p => p.PrivilegeId == privilegeId);
            privilege.ChangeStatus(Status.FromCode(status));
            privilege.SetPriority(priority);
        }
    }
}
