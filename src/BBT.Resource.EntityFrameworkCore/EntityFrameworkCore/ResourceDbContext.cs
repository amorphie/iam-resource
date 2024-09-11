using System;
using BBT.Prism.EntityFrameworkCore;
using BBT.Resource.Policies;
using BBT.Resource.Privileges;
using BBT.Resource.Resources;
using BBT.Resource.Roles;
using BBT.Resource.Rules;
using Microsoft.EntityFrameworkCore;

namespace BBT.Resource.EntityFrameworkCore;

public class ResourceDbContext(
    IServiceProvider serviceProvider,
    DbContextOptions<ResourceDbContext> options
)
    : PrismDbContext<ResourceDbContext>(serviceProvider, options)
{
    public virtual DbSet<ResourceGroup> ResourceGroups { get; set; }
    public virtual DbSet<ResourceGroupTranslation> ResourceGroupTranslations { get; set; }
    public virtual DbSet<Resources.Resource> Resources { get; set; }
    public virtual DbSet<ResourceTranslation> ResourceTranslations { get; set; }
    public virtual DbSet<Rule> Rules { get; set; }
    public virtual DbSet<ResourceRule> ResourceRules { get; set; }
    public virtual DbSet<Privilege> Privileges { get; set; }
    public virtual DbSet<ResourcePrivilege> ResourcePrivileges { get; set; }

    public virtual DbSet<RoleDefinition> RoleDefinitions { get; set; }
    public virtual DbSet<RoleDefinitionTranslation> RoleDefinitionTranslations { get; set; }
    public virtual DbSet<RoleGroup> RoleGroups { get; set; }
    public virtual DbSet<RoleGroupTranslation> RoleGroupTranslations { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<RoleTranslation> RoleTranslations { get; set; }
    public virtual DbSet<RoleGroupRole> RoleGroupRoles { get; set; }
    public virtual DbSet<Scope> Scopes { get; set; }
    public virtual DbSet<ScopeTranslation> ScopeTranslations { get; set; }
    
    public virtual DbSet<Policy> Policies { get; set; }
    public virtual DbSet<ResourcePolicy> ResourcePolicies { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureResource();
    }
}
