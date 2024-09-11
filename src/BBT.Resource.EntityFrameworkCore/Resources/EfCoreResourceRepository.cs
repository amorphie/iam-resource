using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using BBT.Prism.Domain.Entities;
using BBT.Prism.Domain.EntityFrameworkCore;
using BBT.Resource.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BBT.Resource.Resources;

public class EfCoreResourceRepository(ResourceDbContext dbContext, IServiceProvider serviceProvider)
    : EfCoreRepository<ResourceDbContext, Resource, Guid>(dbContext, serviceProvider), IResourceRepository
{
    public override async Task<IQueryable<Resource>> WithDetailsAsync()
    {
        return (await GetQueryableAsync())
            .Include(p => p.Translations);
    }

    public async Task<Resource> GetWithRuleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var resource = await (await GetDbSetAsync())
            .Include(i => i.Rules)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        if (resource == null)
        {
            throw new EntityNotFoundException(typeof(Resource), id);
        }

        return resource;
    }

    public async Task<Resource> GetWithPrivilegeAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var resource = await (await GetDbSetAsync())
            .Include(i => i.Privileges)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        if (resource == null)
        {
            throw new EntityNotFoundException(typeof(Resource), id);
        }

        return resource;
    }

    public async Task<Resource> GetWithPoliciesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var resource = await (await GetDbSetAsync())
            .Include(i => i.Policies)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        if (resource == null)
        {
            throw new EntityNotFoundException(typeof(Resource), id);
        }

        return resource;
    }

    public async Task<List<ResourceRuleModel>> GetRulesAsync(Guid resourceId,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        return await (from related in dbContext.ResourceRules.AsNoTracking()
                where related.ResourceId == resourceId
                join rule in dbContext.Rules.AsNoTracking()
                    on related.RuleId equals rule.Id
                select new ResourceRuleModel
                {
                    Rule = rule,
                    RelatedRule = related
                })
            .OrderBy(o => o.RelatedRule.Priority)
            .ToListAsync(cancellationToken);
    }

    public async Task<ResourceRuleModel> GetRuleAsync(Guid resourceId, Guid ruleId,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var entity = await (from related in dbContext.ResourceRules.AsNoTracking()
                where related.ResourceId == resourceId && related.RuleId == ruleId
                join rule in dbContext.Rules.AsNoTracking()
                    on related.RuleId equals rule.Id
                select new ResourceRuleModel
                {
                    Rule = rule,
                    RelatedRule = related
                })
            .FirstOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            throw new EntityNotFoundException(typeof(ResourceRule), ruleId);
        }

        return entity;
    }

    public async Task<List<ResourcePrivilegeModel>> GetPrivilegesAsync(Guid resourceId,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        return await (from related in dbContext.ResourcePrivileges.AsNoTracking()
                where related.ResourceId == resourceId
                join privilege in dbContext.Privileges.AsNoTracking()
                    on related.PrivilegeId equals privilege.Id
                select new ResourcePrivilegeModel
                {
                    Privilege = privilege,
                    RelatedPrivilege = related
                })
            .OrderBy(o => o.RelatedPrivilege.Priority)
            .ToListAsync(cancellationToken);
    }

    public async Task<ResourcePrivilegeModel> GetPrivilegeAsync(Guid resourceId, Guid privilegeId,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var entity = await (from related in dbContext.ResourcePrivileges.AsNoTracking()
                where related.ResourceId == resourceId && related.PrivilegeId == privilegeId
                join privilege in dbContext.Privileges.AsNoTracking()
                    on related.PrivilegeId equals privilege.Id
                select new ResourcePrivilegeModel
                {
                    Privilege = privilege,
                    RelatedPrivilege = related
                })
            .FirstOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            throw new EntityNotFoundException(typeof(ResourcePrivilege), privilegeId);
        }

        return entity;
    }

    public async Task<List<ResourcePolicyModel>> GetPoliciesAsync(Guid resourceId,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        return await (from related in dbContext.ResourcePolicies.AsNoTracking()
                where related.ResourceId == resourceId
                join policy in dbContext.Policies.AsNoTracking()
                    on related.PolicyId equals policy.Id
                select new ResourcePolicyModel
                {
                    Policy = policy,
                    RelatedPolicy = related
                })
            .OrderBy(o => o.RelatedPolicy.Priority)
            .ToListAsync(cancellationToken);
    }

    public async Task<ResourcePolicyModel> GetPolicyAsync(Guid resourceId, Guid policyId,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var entity = await (from related in dbContext.ResourcePolicies.AsNoTracking()
                where related.ResourceId == resourceId && related.PolicyId == policyId
                join policy in dbContext.Policies.AsNoTracking()
                    on related.PolicyId equals policy.Id
                select new ResourcePolicyModel
                {
                    Policy = policy,
                    RelatedPolicy = related
                })
            .FirstOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            throw new EntityNotFoundException(typeof(ResourcePrivilege), policyId);
        }

        return entity;
    }

    public async Task<Resource?> FindByRegexAsync(string url, ResourceType type,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .AsNoTracking()
            .FirstOrDefaultAsync(p =>
                Regex.IsMatch(url, p.Url)
                && (p.Type == type || p.Type == ResourceType.ALL)
                && p.Status == Status.Active, cancellationToken);
    }
}
