using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BBT.Prism.Domain.Entities.Auditing;
using BBT.Prism.MultiLingualEntities;

namespace BBT.Resource.Resources;

public class ResourceGroup : AuditedEntity<Guid>, IMultiLingualEntity<ResourceGroupTranslation>
{
    public string[]? Tags { get; set; }
    public Status Status { get; set; }
    public ICollection<ResourceGroupTranslation> Translations { get; set; }

    private ResourceGroup()
    {
        //For Orm
    }

    public ResourceGroup(
        Guid id)
        : base(id)
    {
        Status = Status.Active;
        Translations = new Collection<ResourceGroupTranslation>();
    }

    public void AddTranslation(string language, string name)
    {
        if (Translations.All(a => a.Language != language))
        {
            Translations.Add(new ResourceGroupTranslation(Id, language, name));
        }
        else
        {
            var translation = Translations.First(p => p.Language == language);
            translation.Name = name;
        }
    }

    public void RemoveTranslation(ResourceGroupTranslation translation)
    {
        Translations.Remove(translation);
    }
}
