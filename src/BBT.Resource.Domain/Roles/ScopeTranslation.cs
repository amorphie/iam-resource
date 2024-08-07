using System;
using BBT.Prism;
using BBT.Prism.Domain.Entities;
using BBT.Prism.MultiLingualEntities;

namespace BBT.Resource.Roles;

public class ScopeTranslation : Entity, IEntityTranslation
{
    public Guid ScopeId { get; private set; }
    public string Language { get; set; }
    public string Name { get; private set; }

    public override object?[] GetKeys()
    {
        return [ScopeId, Language];
    }

    private ScopeTranslation()
    {
        //For Orm
    }

    internal ScopeTranslation(Guid groupId, string language, string name)
    {
        ScopeId = groupId;
        Language = Check.NotNullOrEmpty(language, nameof(Language), SharedConsts.MaxLanguageLength);
        SetName(name);
    }

    internal void SetName(string name)
    {
        Name = Check.NotNullOrEmpty(name, nameof(Name), ScopeConsts.MaxNameLength);
    }
}
