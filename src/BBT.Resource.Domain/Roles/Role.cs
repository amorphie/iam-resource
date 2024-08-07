using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BBT.Prism.Domain.Entities.Auditing;
using BBT.Prism.MultiLingualEntities;

namespace BBT.Resource.Roles;

public class Role: AuditedEntity<Guid>, IMultiLingualEntity<RoleTranslation>
{
    public Guid DefinitionId { get; private set; }
    public string[]? Tags { get; set; }
    public Status Status { get; private set; }
    public ICollection<RoleTranslation> Translations { get; set; }
    
    private Role()
    {
        //For Orm
    }

    public Role(
        Guid id,
        Guid definitionId)
        : base(id)
    {
        DefinitionId = definitionId;
        Status = Status.Active;

        Translations = new Collection<RoleTranslation>();
    }

    public void ChangeStatus(string status)
    {
        Status = Status.FromCode(status);
    }
    
    public void AddTranslation(string language, string name)
    {
        if (Translations.All(a => a.Language != language))
        {
            Translations.Add(new RoleTranslation(Id, language, name));
        }
        else
        {
            var translation = Translations.First(p => p.Language == language);
            translation.SetName(name);
        }
    }

    public void RemoveTranslation(RoleTranslation translation)
    {
        Translations.Remove(translation);
    }
}
