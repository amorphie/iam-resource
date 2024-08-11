using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BBT.Prism.Domain.Entities.Auditing;
using BBT.Prism.MultiLingualEntities;

namespace BBT.Resource.Roles;

public class RoleGroup : AuditedAggregateRoot<Guid>, IMultiLingualEntity<RoleGroupTranslation>
{
    public string[]? Tags { get; set; }
    public Status Status { get; private set; }
    public ICollection<RoleGroupTranslation> Translations { get; set; }
    public ICollection<RoleGroupRole> Roles { get; private set; }

    private RoleGroup()
    {
        //For Orm
    }

    public RoleGroup(
        Guid id)
        : base(id)
    {
        Status = Status.Active;

        Roles = new Collection<RoleGroupRole>();
        Translations = new Collection<RoleGroupTranslation>();
    }
    
    public void ChangeStatus(string status)
    {
        Status = Status.FromCode(status);
    }

    public void AddTranslation(string language, string name)
    {
        if (Translations.All(a => a.Language != language))
        {
            Translations.Add(new RoleGroupTranslation(Id, language, name));
        }
        else
        {
            var translation = Translations.First(p => p.Language == language);
            translation.SetName(name);
        }
    }

    public void RemoveTranslation(RoleGroupTranslation translation)
    {
        Translations.Remove(translation);
    }

    public void AddRole(Guid roleId)
    {
        if (Roles.All(a => a.RoleId != roleId))
        {
            Roles.Add(new RoleGroupRole(Id, roleId));
        }
    }

    public void RemoveRole(Guid roleId)
    {
        if (Roles.Any(a => a.RoleId == roleId))
        {
            Roles.Remove(Roles.First(p => p.RoleId == roleId));
        }
    }

    public void ChangeRoleStatus(Guid roleId, string status)
    {
        if (Roles.Any(a => a.RoleId == roleId))
        {
            var role = Roles.First(p => p.RoleId == roleId);
            role.ChangeStatus(Status.FromCode(status));
        }
    }
}
