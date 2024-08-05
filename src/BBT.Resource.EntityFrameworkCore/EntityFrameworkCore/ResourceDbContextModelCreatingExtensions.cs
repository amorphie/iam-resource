using BBT.Prism.EntityFrameworkCore.Modeling;
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

            b.HasMany(p => p.Translations)
                .WithOne()
                .HasForeignKey(p => p.GroupId);
        });

        builder.Entity<ResourceGroupTranslation>(b =>
        {
            b.ToTable("ResourceGroupTranslations");
            b.ConfigureByConvention();
            b.HasKey(k => new { k.Language, k.GroupId });

            b.Property(p => p.Language).IsRequired().HasMaxLength(SharedConsts.MaxLanguageLength);
            b.Property(p => p.Name).IsRequired().HasMaxLength(ResourceGroupConsts.MaxNameLength);
        });

        builder.Entity<Resources.Resource>(b =>
        {
            b.ToTable("Resources");
            b.ConfigureByConvention();

            b.Property(p => p.Status).IsRequired().HasMaxLength(SharedConsts.MaxStatusLength);
            b.Property(p => p.Url).IsRequired().HasMaxLength(ResourceConsts.MaxUrlLength);

            b.HasMany(p => p.Translations)
                .WithOne()
                .HasForeignKey(p => p.ResourceId);

            b.HasMany(p => p.Roles)
                .WithOne()
                .HasForeignKey(p => p.ResourceId);

            b.HasMany(p => p.Rules)
                .WithOne()
                .HasForeignKey(p => p.ResourceId);

            b.HasMany(p => p.Privileges)
                .WithOne()
                .HasForeignKey(p => p.ResourceId);

            b.HasMany(p => p.Privileges)
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

        builder.Entity<ResourceRole>(b =>
        {
            b.ToTable("ResourceRoles");
            b.ConfigureByConvention();

            b.HasKey(k => new { k.ResourceId, k.RoleId, k.Status });
            b.Property(p => p.Status).IsRequired().HasMaxLength(SharedConsts.MaxStatusLength);
        });

        builder.Entity<ResourceRule>(b =>
        {
            b.ToTable("ResourceRules");
            b.ConfigureByConvention();

            b.HasKey(k => new { k.ResourceId, k.RuleId, k.ClientId, k.Status });
            b.Property(p => p.Status).IsRequired().HasMaxLength(SharedConsts.MaxStatusLength);
        });

        builder.Entity<ResourcePrivilege>(b =>
        {
            b.ToTable("ResourcePrivileges");
            b.ConfigureByConvention();

            b.HasKey(k => new { k.ResourceId, k.Priority, k.ClientId, k.Status });
            b.Property(p => p.Status).IsRequired().HasMaxLength(SharedConsts.MaxStatusLength);
        });

        #endregion

        #region Privileges

        builder.Entity<Privilege>(b =>
        {
            b.ToTable("Privileges");
            b.ConfigureByConvention();

            b.Property(p => p.Url).IsRequired().HasMaxLength(PrivilegeConsts.MaxUrlLength);

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

            b.Property(p => p.Name).IsRequired().HasMaxLength(RuleConsts.MaxNameLength);

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

            b.Property(p => p.Key).IsRequired().HasMaxLength(RoleDefinitionConsts.MaxKeyLength);
            b.Property(p => p.ClientId).IsRequired();
            b.Property(p => p.Status).IsRequired().HasMaxLength(SharedConsts.MaxStatusLength);

            b.HasIndex(i => new { i.Key, i.ClientId, i.Status }).IsUnique();
            
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

            b.Property(p => p.Language).IsRequired().HasMaxLength(SharedConsts.MaxLanguageLength);
            b.Property(p => p.Name).IsRequired().HasMaxLength(RoleDefinitionConsts.MaxNameLength);
            b.Property(p => p.Description).HasMaxLength(RoleDefinitionConsts.MaxNameLength);
        });

        builder.Entity<RoleGroup>(b =>
        {
            b.ToTable("RoleGroups");
            b.ConfigureByConvention();

            b.Property(p => p.Status).IsRequired().HasMaxLength(SharedConsts.MaxStatusLength);

            b.HasMany(p => p.Translations)
                .WithOne()
                .HasForeignKey(p => p.GroupId);

            b.HasMany(p => p.Roles)
                .WithOne()
                .HasForeignKey(p => p.GroupId);

            b.HasMany(p => p.Scopes)
                .WithOne()
                .HasForeignKey(p => p.GroupId);
        });

        builder.Entity<RoleGroupTranslation>(b =>
        {
            b.ToTable("RoleGroupTranslations");
            b.ConfigureByConvention();

            b.HasKey(k => new { k.Language, k.GroupId });

            b.Property(p => p.Language).IsRequired().HasMaxLength(SharedConsts.MaxLanguageLength);
            b.Property(p => p.Name).IsRequired().HasMaxLength(RoleGroupConsts.MaxNameLength);
        });

        builder.Entity<Role>(b =>
        {
            b.ToTable("Roles");
            b.ConfigureByConvention();

            b.Property(p => p.Status).IsRequired().HasMaxLength(SharedConsts.MaxStatusLength);

            b.HasMany(p => p.Translations)
                .WithOne()
                .HasForeignKey(p => p.RoleId);

            b.HasMany<RoleGroupRole>()
                .WithOne()
                .HasForeignKey(p => p.RoleId);

            b.HasMany<ResourceRole>()
                .WithOne()
                .HasForeignKey(p => p.RoleId);
        });

        builder.Entity<RoleTranslation>(b =>
        {
            b.ToTable("RoleTranslations");
            b.ConfigureByConvention();

            b.HasKey(k => new { k.Language, k.RoleId });

            b.Property(p => p.Language).IsRequired().HasMaxLength(SharedConsts.MaxLanguageLength);
            b.Property(p => p.Name).IsRequired().HasMaxLength(RoleConsts.MaxNameLength);
        });

        builder.Entity<RoleGroupRole>(b =>
        {
            b.ToTable("RoleGroupRoles");
            b.ConfigureByConvention();
            b.HasKey(k => new { k.GroupId, k.RoleId, k.Status });

            b.Property(p => p.Status).IsRequired().HasMaxLength(SharedConsts.MaxStatusLength);
        });

        builder.Entity<Scope>(b =>
        {
            b.ToTable("Scopes");
            b.ConfigureByConvention();

            b.Property(p => p.Reference).HasMaxLength(ScopeConsts.MaxReferenceLength);
            b.Property(p => p.Status).IsRequired().HasMaxLength(SharedConsts.MaxStatusLength);
        });

        #endregion
    }
}
