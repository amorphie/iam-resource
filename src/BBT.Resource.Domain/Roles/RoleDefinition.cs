using System;
using System.Collections.Generic;
using System.Linq;
using BBT.Prism;
using BBT.Prism.Domain.Entities.Auditing;
using BBT.Prism.MultiLingualEntities;

namespace BBT.Resource.Roles;

public class RoleDefinition : AuditedEntity<Guid>,  IMultiLingualEntity<RoleDefinitionTranslation>
{
    public string Key { get; private set; }
    public Guid ClientId { get; set; }
    public string[]? Tags { get; set; }
    public Status Status { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public ICollection<RoleDefinitionTranslation> Translations { get; set; }

    private RoleDefinition()
    {
        //For Orm
    }

    public RoleDefinition(
        Guid id,
        string key,
        Guid clientId)
        : base(id)
    {
        Key = Check.NotNullOrEmpty(key, nameof(Key), RoleDefinitionConsts.MaxKeyLength);
        ClientId = clientId;
        Status = Status.Active;
    }

    public void SetKey(string key)
    {
        Key = Check.NotNullOrEmpty(key, nameof(Key), RoleDefinitionConsts.MaxKeyLength);
    }

    public void AddTranslation(string language, string name, string? description)
    {
        if (Translations.All(a => a.Language != language))
        {
            Translations.Add(new RoleDefinitionTranslation(Id, language, name, description));
        }
        else
        {
            var translation = Translations.First(p => p.Language == language);
            translation.Name = name;
            translation.Description = description;
        }
    }

    public void RemoveTranslation(RoleDefinitionTranslation translation)
    {
        Translations.Remove(translation);
    }
}
