using BBT.Prism.EntityFrameworkCore.Modeling;
using BBT.Resource.EntityFrameworkCore.ValueConverters;
using BBT.Resource.Permissions;
using BBT.Resource.Policies;
using BBT.Resource.Privileges;
using BBT.Resource.Resources;
using BBT.Resource.Roles;
using BBT.Resource.Rules;
using Microsoft.EntityFrameworkCore;

namespace BBT.Resource.EntityFrameworkCore;

public static class ResourceDbContextModelCreatingExtensions
{
    public static void ConfigureResource(
        this ModelBuilder builder)
    {
        /* Configure all entities here. */

        #region Resources

        builder.Entity<ResourceGroup>(b =>
        {
            b.ToTable("ResourceGroups");
            b.ConfigureByConvention();

            b.Property(p => p.Status)
                .IsRequired()
                .HasMaxLength(SharedConsts.MaxStatusLength)
                .HasConversion(new StatusConverter());

            b.HasMany(p => p.Translations)
                .WithOne()
                .HasForeignKey(p => p.GroupId);
        });

        builder.Entity<ResourceGroupTranslation>(b =>
        {
            b.ToTable("ResourceGroupTranslations");
            b.ConfigureByConvention();
            b.HasKey(k => new { k.Language, k.GroupId });

            b.Property(p => p.Language)
                .IsRequired()
                .HasMaxLength(SharedConsts.MaxLanguageLength);

            b.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(ResourceGroupConsts.MaxNameLength);
        });

        builder.Entity<Resources.Resource>(b =>
        {
            b.ToTable("Resources");
            b.ConfigureByConvention();

            b.Property(p => p.Status)
                .IsRequired()
                .HasMaxLength(SharedConsts.MaxStatusLength)
                .HasConversion(new StatusConverter());

            b.Property(p => p.Url)
                .IsRequired()
                .HasMaxLength(ResourceConsts.MaxUrlLength);

            b.HasIndex(i => new { i.Url, i.Type }).IsUnique();

            b.HasMany(p => p.Translations)
                .WithOne()
                .HasForeignKey(p => p.ResourceId);

            b.HasMany(p => p.Rules)
                .WithOne()
                .HasForeignKey(p => p.ResourceId);

            b.HasMany(p => p.Privileges)
                .WithOne()
                .HasForeignKey(p => p.ResourceId);

            b.HasMany(p => p.Policies)
                .WithOne()
                .HasForeignKey(p => p.ResourceId);
        });

        builder.Entity<ResourceTranslation>(b =>
        {
            b.ToTable("ResourceTranslations");
            b.ConfigureByConvention();

            b.HasKey(k => new { k.Language, k.ResourceId });

            b.Property(p => p.Language).IsRequired().HasMaxLength(SharedConsts.MaxLanguageLength);
            b.Property(p => p.Name).IsRequired().HasMaxLength(ResourceConsts.MaxNameLength);
            b.Property(p => p.Description).HasMaxLength(ResourceConsts.MaxDescriptionLength);
        });

        builder.Entity<ResourceRule>(b =>
        {
            b.ToTable("ResourceRules");
            b.ConfigureByConvention();

            //TODO: ClientId is currently nullable, so it is not added to the index.
            b.HasKey(k => new { k.ResourceId, k.RuleId });
            b.Property(p => p.Status)
                .IsRequired()
                .HasMaxLength(SharedConsts.MaxStatusLength)
                .HasConversion(new StatusConverter());
        });

        builder.Entity<ResourcePolicy>(b =>
        {
            b.ToTable("ResourcePolicies");
            b.ConfigureByConvention();

            b.HasKey(k => new { k.ResourceId, k.PolicyId });

            b.Property(p => p.Status)
                .IsRequired()
                .HasMaxLength(SharedConsts.MaxStatusLength)
                .HasConversion(new StatusConverter());
        });

        builder.Entity<ResourcePrivilege>(b =>
        {
            b.ToTable("ResourcePrivileges");
            b.ConfigureByConvention();

            //TODO: ClientId is currently nullable, so it is not added to the index.
            b.HasKey(k => new { k.ResourceId, k.Priority });
            b.Property(p => p.Status)
                .IsRequired()
                .HasMaxLength(SharedConsts.MaxStatusLength)
                .HasConversion(new StatusConverter());
        });

        #endregion

        #region Privileges

        builder.Entity<Privilege>(b =>
        {
            b.ToTable("Privileges");
            b.ConfigureByConvention();

            b.Property(p => p.Url)
                .IsRequired()
                .HasMaxLength(PrivilegeConsts.MaxUrlLength);

            b.HasMany<ResourcePrivilege>()
                .WithOne()
                .HasForeignKey(p => p.PrivilegeId);
        });

        #endregion

        #region Rules

        builder.Entity<Rule>(b =>
        {
            b.ToTable("Rules");
            b.ConfigureByConvention();

            b.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(RuleConsts.MaxNameLength);

            b.HasMany<ResourceRule>()
                .WithOne()
                .HasForeignKey(p => p.RuleId);
        });

        #endregion

        #region Roles

        builder.Entity<RoleDefinition>(b =>
        {
            b.ToTable("RoleDefinitions");
            b.ConfigureByConvention();

            b.Property(p => p.Key)
                .IsRequired()
                .HasMaxLength(RoleDefinitionConsts.MaxKeyLength);

            b.Property(p => p.ClientId)
                .IsRequired();

            b.Property(p => p.Tags);

            b.Property(p => p.Status)
                .IsRequired()
                .HasMaxLength(SharedConsts.MaxStatusLength)
                .HasConversion(new StatusConverter());

            b.HasIndex(i => new { i.Key, i.ClientId })
                .IsUnique();

            b.HasMany(p => p.Translations)
                .WithOne()
                .HasForeignKey(p => p.DefinitionId);

            b.HasMany<Role>()
                .WithOne()
                .HasForeignKey(p => p.DefinitionId);
        });

        builder.Entity<RoleDefinitionTranslation>(b =>
        {
            b.ToTable("RoleDefinitionTranslations");
            b.ConfigureByConvention();

            b.HasKey(k => new { k.Language, k.DefinitionId });

            b.Property(p => p.Language)
                .IsRequired()
                .HasMaxLength(SharedConsts.MaxLanguageLength);

            b.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(RoleDefinitionConsts.MaxNameLength);

            b.Property(p => p.Description)
                .HasMaxLength(RoleDefinitionConsts.MaxNameLength);
        });

        builder.Entity<RoleGroup>(b =>
        {
            b.ToTable("RoleGroups");
            b.ConfigureByConvention();

            b.Property(p => p.Status)
                .IsRequired()
                .HasMaxLength(SharedConsts.MaxStatusLength)
                .HasConversion(new StatusConverter());

            b.HasMany(p => p.Translations)
                .WithOne()
                .HasForeignKey(p => p.GroupId);

            b.HasMany(p => p.Roles)
                .WithOne()
                .HasForeignKey(p => p.GroupId);

            b.HasMany<Scope>()
                .WithOne()
                .HasForeignKey(p => p.GroupId);
        });

        builder.Entity<RoleGroupTranslation>(b =>
        {
            b.ToTable("RoleGroupTranslations");
            b.ConfigureByConvention();

            b.HasKey(k => new { k.Language, k.GroupId });

            b.Property(p => p.Language)
                .IsRequired()
                .HasMaxLength(SharedConsts.MaxLanguageLength);

            b.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(RoleGroupConsts.MaxNameLength);
        });

        builder.Entity<Role>(b =>
        {
            b.ToTable("Roles");
            b.ConfigureByConvention();

            b.Property(p => p.Status)
                .IsRequired()
                .HasMaxLength(SharedConsts.MaxStatusLength)
                .HasConversion(new StatusConverter());

            b.HasMany(p => p.Translations)
                .WithOne()
                .HasForeignKey(p => p.RoleId);

            b.HasMany<RoleGroupRole>()
                .WithOne()
                .HasForeignKey(p => p.RoleId);
        });

        builder.Entity<RoleTranslation>(b =>
        {
            b.ToTable("RoleTranslations");
            b.ConfigureByConvention();

            b.HasKey(k => new { k.Language, k.RoleId });

            b.Property(p => p.Language)
                .IsRequired()
                .HasMaxLength(SharedConsts.MaxLanguageLength);

            b.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(RoleConsts.MaxNameLength);
        });

        builder.Entity<RoleGroupRole>(b =>
        {
            b.ToTable("RoleGroupRoles");
            b.ConfigureByConvention();
            b.HasKey(k => new { k.GroupId, k.RoleId });

            b.Property(p => p.Status)
                .IsRequired()
                .HasMaxLength(SharedConsts.MaxStatusLength)
                .HasConversion(new StatusConverter());
        });

        builder.Entity<Scope>(b =>
        {
            b.ToTable("Scopes");
            b.ConfigureByConvention();

            b.Property(p => p.Reference)
                .HasMaxLength(ScopeConsts.MaxReferenceLength);

            b.Property(p => p.Status)
                .IsRequired()
                .HasMaxLength(SharedConsts.MaxStatusLength)
                .HasConversion(new StatusConverter());
        });

        builder.Entity<ScopeTranslation>(b =>
        {
            b.ToTable("ScopeTranslations");
            b.ConfigureByConvention();

            b.HasKey(k => new { k.Language, k.ScopeId });

            b.Property(p => p.Language)
                .IsRequired()
                .HasMaxLength(SharedConsts.MaxLanguageLength);

            b.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(RoleConsts.MaxNameLength);
        });

        #endregion

        #region Policies

        builder.Entity<Policy>(b =>
        {
            b.ToTable("Policies");
            b.ConfigureByConvention();

            b.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(PolicyConsts.MaxNameLength);

            b.Property(p => p.ParentId);
            b.Property(p => p.Priority);
            b.Property(p => p.Permissions);
            b.Property(p => p.PermissionProvider)
                .HasMaxLength(PolicyConsts.MaxProviderNameLength);
            b.Property(p => p.EvaluationOrder);

            b.Property(p => p.Effect)
                .IsRequired()
                .HasMaxLength(PolicyConsts.MaxEffectLength)
                .HasConversion(new EffectConverter());

            b.Property(p => p.ConflictResolution)
                .IsRequired()
                .HasDefaultValue(ConflictResolution.NA)
                .HasMaxLength(PolicyConsts.MaxConflictResolutionLength)
                .HasConversion(new ConflictResolutionConverter());

            b.HasMany<ResourcePolicy>()
                .WithOne()
                .HasForeignKey(p => p.PolicyId);

            b.OwnsOne(p => p.Condition, cnd =>
            {
                cnd.ToTable("PolicyConditions");
                cnd.WithOwner();
                cnd.Property(t => t.Roles);
                cnd.Property(t => t.Rules);
                cnd.OwnsOne(c => c.Time, time =>
                {
                    time.Property(t => t.Start)
                        .HasColumnName("StartTime");
                    time.Property(t => t.End)
                        .HasColumnName("EndTime");
                    time.Property(t => t.Timezone)
                        .HasColumnName("Timezone")
                        .HasMaxLength(PolicyConsts.MaxTimezoneLength);
                });

                // ObjectDictionary fields as JSON
                cnd.Property(t => t.Context)
                    .HasConversion(new ObjectDictionaryConverter())
                    .HasColumnType("jsonb");

                cnd.Property(t => t.Attributes)
                    .HasConversion(new ObjectDictionaryConverter())
                    .HasColumnType("jsonb");
            });
        });

        #endregion

        #region Permissions

        builder.Entity<Permission>(b =>
        {
            b.ToTable("Permissions");
            b.ConfigureByConvention();

            b.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(PermissionsConsts.MaxNameLength);

            b.Property(p => p.ApplicationId)
                .IsRequired()
                .HasMaxLength(PermissionsConsts.MaxApplicationIdLength);

            b.Property(p => p.ClientId)
                .IsRequired()
                .HasMaxLength(PermissionsConsts.MaxClientIdLength);

            b.HasMany(p => p.Translations)
                .WithOne()
                .HasForeignKey(p => p.PermissionId);
        });

        builder.Entity<PermissionTranslation>(b =>
        {
            b.ToTable("PermissionTranslations");
            b.ConfigureByConvention();

            b.HasKey(k => new { k.Language, k.PermissionId });

            b.Property(p => p.Language)
                .IsRequired()
                .HasMaxLength(SharedConsts.MaxLanguageLength);

            b.Property(p => p.DisplayName)
                .IsRequired()
                .HasMaxLength(PermissionsConsts.MaxDisplayNameLength);

            b.Property(p => p.Description)
                .HasMaxLength(PermissionsConsts.MaxDescriptionLength);
        });

        builder.Entity<PermissionGrant>(b =>
        {
            b.ToTable("PermissionGrants");
            b.ConfigureByConvention();

            b.HasIndex(i => new
            {
                i.ApplicationId,
                i.ClientId,
                i.Name,
                i.ProviderName,
                i.ProviderKey
            }).IsUnique();

            b.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(PermissionsConsts.MaxNameLength);

            b.Property(p => p.ClientId)
                .IsRequired()
                .HasMaxLength(PermissionsConsts.MaxClientIdLength);

            b.Property(p => p.ApplicationId)
                .IsRequired()
                .HasMaxLength(PermissionsConsts.MaxApplicationIdLength);

            b.Property(p => p.ProviderKey)
                .IsRequired()
                .HasMaxLength(PermissionsConsts.MaxProviderKeyLength);

            b.Property(p => p.ProviderName)
                .IsRequired()
                .HasMaxLength(PermissionsConsts.MaxProviderNameLength);
        });

        #endregion
    }
}
