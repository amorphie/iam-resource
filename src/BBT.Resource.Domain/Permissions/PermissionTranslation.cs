using System;
using BBT.Prism;
using BBT.Prism.Domain.Entities;
using BBT.Prism.MultiLingualEntities;
using BBT.Resource.Roles;

namespace BBT.Resource.Permissions;

public class PermissionTranslation : Entity, IEntityTranslation
{
    public Guid PermissionId { get; private set; }
    public string Language { get; set; }
    public string DisplayName { get; private set; }
    public string Description { get; private set; }

    public override object?[] GetKeys()
    {
        return [PermissionId, Language];
    }

    private PermissionTranslation()
    {
        //For Orm
    }

    internal PermissionTranslation(
        Guid permissionId,
        string language,
        string name,
        string description)
    {
        PermissionId = permissionId;
        Language = Check.NotNullOrEmpty(language, nameof(Language), SharedConsts.MaxLanguageLength);
        SetName(name);
        SetDescription(description);
    }

    internal void SetName(string name)
    {
        DisplayName = Check.NotNullOrEmpty(name, nameof(DisplayName), PermissionsConsts.MaxDisplayNameLength);
    }

    internal void SetDescription(string description)
    {
        Description = Check.NotNullOrEmpty(description, nameof(Description), PermissionsConsts.MaxDescriptionLength);
    }
}
