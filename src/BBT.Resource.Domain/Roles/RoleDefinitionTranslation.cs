using System;
using BBT.Prism;
using BBT.Prism.Domain.Entities;
using BBT.Prism.MultiLingualEntities;
using BBT.Resource.Resources;

namespace BBT.Resource.Roles;

public class RoleDefinitionTranslation : Entity, IEntityTranslation
{
    public Guid DefinitionId { get; set; }
    public string Language { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }

    public override object?[] GetKeys()
    {
        return [DefinitionId, Language];
    }

    private RoleDefinitionTranslation()
    {
        //For Orm
    }

    internal RoleDefinitionTranslation(Guid definition, string language, string name, string? description)
    {
        DefinitionId = definition;
        Language = Check.NotNullOrEmpty(language, nameof(Language), SharedConsts.MaxLanguageLength);
        Name = Check.NotNullOrEmpty(name, nameof(Name), RoleDefinitionConsts.MaxNameLength);
        Description = Check.Length(description, nameof(Description), RoleDefinitionConsts.MaxDescriptionLength);
    }
}
