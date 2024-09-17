using System;
using System.Collections.Generic;
using System.Linq;
using BBT.Prism;
using BBT.Prism.Domain.Entities.Auditing;
using BBT.Prism.MultiLingualEntities;

namespace BBT.Resource.Permissions;

public class Permission : AuditedEntity<Guid>, IMultiLingualEntity<PermissionTranslation>
{
    /// <summary>
    /// The name of the permission, structured in a specific format.
    /// </summary>
    /// <remarks>
    /// Pattern: <c>ModuleName.Domain.Transaction</c>  
    /// Example: <c>Identity.Users.Create</c>  
    /// 
    /// Format breakdown:
    /// <list type="bullet">
    ///   <item>
    ///     <term>ModuleName: </term>
    ///     <description>Refers to the module or context (e.g., Identity)</description>
    ///   </item>
    ///   <item>
    ///     <term>Domain: </term>
    ///     <description>Refers to the specific domain within the module (e.g., Users)</description>
    ///   </item>
    ///   <item>
    ///     <term>Transaction: </term>
    ///     <description>Indicates the action or permission type (e.g., Create)</description>
    ///   </item>
    /// </list>
    /// </remarks>
    public string Name { get; private set; }

    public string ApplicationId { get; private set; }

    /// <summary>
    /// Client Id
    /// </summary>
    public string ClientId { get; private set; }

    public Guid? ParentId { get; private set; }
    public ICollection<PermissionTranslation> Translations { get; set; }

    private Permission()
    {
        // For ORM
    }

    public Permission(
        Guid id,
        string applicationId,
        string clientId,
        string name,
        Guid? parentId = null)
        : base(id)
    {
        ApplicationId =
            Check.NotNullOrEmpty(applicationId, nameof(ApplicationId), PermissionsConsts.MaxApplicationIdLength);
        Name = Check.NotNullOrEmpty(name, nameof(Name), PermissionsConsts.MaxNameLength);
        ParentId = parentId;
        ClientId = Check.NotNullOrEmpty(clientId, nameof(ClientId), PermissionsConsts.MaxClientIdLength);
    }

    public void AddTranslation(string language, string name, string description)
    {
        if (Translations.All(a => a.Language != language))
        {
            Translations.Add(new PermissionTranslation(Id, language, name, description));
        }
        else
        {
            var translation = Translations.First(p => p.Language == language);
            translation.SetName(name);
            translation.SetDescription(description);
        }
    }

    public void RemoveTranslation(PermissionTranslation translation)
    {
        Translations.Remove(translation);
    }
    
    public static string? ExtractParentName(string permissionName)
    {
        var parts = permissionName.Split('.');
        if (parts.Length > 1)
        {
            return string.Join('.', parts.Take(parts.Length - 1));
        }

        return null;
    }
}
