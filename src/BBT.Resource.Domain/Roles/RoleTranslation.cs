using System;
using BBT.Prism;
using BBT.Prism.Domain.Entities;
using BBT.Prism.MultiLingualEntities;

namespace BBT.Resource.Roles;

public class RoleTranslation : Entity, IEntityTranslation
{
    public Guid RoleId { get; private set; }
    public string Language { get; set; }
    public string Name { get; private set; }

    public override object?[] GetKeys()
    {
        return [RoleId, Language];
    }

    private RoleTranslation()
    {
        //For Orm
    }

    internal RoleTranslation(
        Guid roleId,
        string language,
        string name)
    {
        RoleId = roleId;
        Language = Check.NotNullOrEmpty(language, nameof(Language), SharedConsts.MaxLanguageLength);
        SetName(name);
    }

    internal void SetName(string name)
    {
        Name = Check.NotNullOrEmpty(name, nameof(Name), RoleConsts.MaxNameLength);
    }
}
