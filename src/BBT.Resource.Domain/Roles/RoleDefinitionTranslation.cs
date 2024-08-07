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
    public string Name { get; private set; }
    public string? Description { get; private set; }

    public override object?[] GetKeys()
    {
        return [DefinitionId, Language];
    }

    private RoleDefinitionTranslation()
    {
        //For Orm
    }

    internal RoleDefinitionTranslation(Guid definitionId, string language, string name, string? description)
    {
        DefinitionId = definitionId;
        Language = Check.NotNullOrEmpty(language, nameof(Language), SharedConsts.MaxLanguageLength);
        SetName(name);
        SetDescription(description);
    }

    internal void SetName(string name)
    {
        Name = Check.NotNullOrEmpty(name, nameof(Name), RoleDefinitionConsts.MaxNameLength);
    }
    
    internal void SetDescription(string? description)
    {
        Description = Check.Length(description, nameof(Description), RoleDefinitionConsts.MaxDescriptionLength);
    }
}
