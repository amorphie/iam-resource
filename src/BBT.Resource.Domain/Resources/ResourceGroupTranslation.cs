using System;
using BBT.Prism;
using BBT.Prism.Domain.Entities;
using BBT.Prism.MultiLingualEntities;

namespace BBT.Resource.Resources;

public class ResourceGroupTranslation : Entity, IEntityTranslation
{
    public Guid GroupId { get; private set; }
    public string Language { get; set; }
    public string Name { get; internal set; }

    public override object?[] GetKeys()
    {
        return [GroupId, Language];
    }

    private ResourceGroupTranslation()
    {
        //For Orm
    }

    internal ResourceGroupTranslation(Guid groupId, string language, string name)
    {
        GroupId = groupId;
        Language = Check.NotNullOrEmpty(language, nameof(Language), SharedConsts.MaxLanguageLength);
        Name = Check.NotNullOrEmpty(name, nameof(Name), ResourceGroupConsts.MaxNameLength);
    }
}
