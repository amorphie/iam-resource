using System;
using BBT.Prism;
using BBT.Prism.Domain.Entities;
using BBT.Prism.MultiLingualEntities;

namespace BBT.Resource.Resources;

public class ResourceTranslation : Entity, IEntityTranslation
{
    public Guid ResourceId { get; private set; }
    public string Language { get; set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }

    public override object?[] GetKeys()
    {
        return [ResourceId, Language];
    }

    private ResourceTranslation()
    {
        //For Orm
    }

    internal ResourceTranslation(Guid groupId, string language, string name, string? description)
    {
        ResourceId = groupId;
        Language = Check.NotNullOrEmpty(language, nameof(Language), SharedConsts.MaxLanguageLength);
        SetName(name);
        SetDescription(description);
    }

    internal void SetName(string name)
    {
        Name = Check.NotNullOrEmpty(name, nameof(Name), ResourceConsts.MaxNameLength);
    }

    internal void SetDescription(string? description)
    {
        Description = Check.NotNullOrEmpty(description, nameof(Description), ResourceConsts.MaxDescriptionLength);
    }
}
