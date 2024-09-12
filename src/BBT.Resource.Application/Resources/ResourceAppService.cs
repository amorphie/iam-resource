using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BBT.Prism.Application.Dtos;
using BBT.Prism.Application.Services;
using BBT.Prism.Domain.Repositories;
using BBT.Prism.Uow;

namespace BBT.Resource.Resources;

public class ResourceAppService(
    IServiceProvider serviceProvider,
    IUnitOfWork unitOfWork,
    IResourceRepository resourceRepository,
    MultiLingualResourceObjectMapper multiLingualMapper,
    ResourceManager resourceManager
)
    : ApplicationService(serviceProvider), IResourceAppService
{
    public async Task<PagedResultDto<ResourceMultiLingualDto>> GetAllAsync(PagedResourceInput input,
        CancellationToken cancellationToken = default)
    {
        var totalCount = await resourceRepository.LongCountAsync(cancellationToken);
        var items = await resourceRepository.GetPagedListAsync(input.SkipCount, input.MaxResultCount,
            input.Sorting, true, cancellationToken);

        var roleDefDtos = items.Select(multiLingualMapper.Map)
            .ToList();

        return new PagedResultDto<ResourceMultiLingualDto>
        {
            TotalCount = totalCount,
            Items = roleDefDtos
        };
    }

    public async Task<ResourceMultiLingualDto> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var resource = await resourceRepository.GetAsync(id, true, cancellationToken);
        return multiLingualMapper.Map(resource);
    }

    public async Task<ResourceDto> CreateAsync(CreateResourceInput input,
        CancellationToken cancellationToken = default)
    {
        var resource = new Resource(
            input.Id,
            input.GroupId,
            input.Type,
            input.Url)
        {
            Tags = input.Tags
        };

        foreach (var translation in input.Translations)
        {
            resource.AddTranslation(translation.Language, translation.Name, translation.Description);
        }

        await resourceRepository.InsertAsync(resource, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return ObjectMapper.Map<Resource, ResourceDto>(resource);
    }

    public async Task<ResourceDto> UpdateAsync(Guid id, UpdateResourceInput input,
        CancellationToken cancellationToken = default)
    {
        var resource = await resourceRepository.GetAsync(id, true, cancellationToken);
        resource.SetUrl(input.Url);
        resource.Type = input.Type;
        resource.Tags = input.Tags;
        resource.ChangeStatus(input.Status);

        foreach (var translation in input.Translations)
        {
            resource.AddTranslation(translation.Language, translation.Name, translation.Description);
        }

        var translationsToRemove = new List<ResourceTranslation>();
        foreach (var translation in resource.Translations)
        {
            if (!input.Translations.Exists(a => a.Language == translation.Language))
            {
                translationsToRemove.Add(translation);
            }
        }

        foreach (var translation in translationsToRemove)
        {
            resource.RemoveTranslation(translation);
        }

        await resourceRepository.UpdateAsync(resource, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return ObjectMapper.Map<Resource, ResourceDto>(resource);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await resourceRepository.DeleteAsync(id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<ListResultDto<ResourceRuleDto>> GetRulesAsync(Guid resourceId,
        CancellationToken cancellationToken = default)
    {
        var resourceRules = await resourceRepository.GetRulesAsync(resourceId, cancellationToken);
        var items = resourceRules.Select(resourceRule =>
            new ResourceRuleDto()
            {
                RuleId = resourceRule.RelatedRule.RuleId,
                ClientId = resourceRule.RelatedRule.ClientId,
                Status = resourceRule.RelatedRule.Status,
                Priority = resourceRule.RelatedRule.Priority,
                CreatedAt = resourceRule.RelatedRule.CreatedAt,
                ModifiedAt = resourceRule.RelatedRule.ModifiedAt,
                RuleName = resourceRule.Rule.Name,
                RuleExpression = resourceRule.Rule.Expression
            }).ToList();
        return new ListResultDto<ResourceRuleDto>(items);
    }

    public async Task<ResourceRuleDto> GetRuleAsync(Guid resourceId, Guid ruleId,
        CancellationToken cancellationToken = default)
    {
        var resourceRule = await resourceRepository.GetRuleAsync(resourceId, ruleId, cancellationToken);
        return new ResourceRuleDto()
        {
            RuleId = resourceRule.RelatedRule.RuleId,
            ClientId = resourceRule.RelatedRule.ClientId,
            CreatedAt = resourceRule.RelatedRule.CreatedAt,
            ModifiedAt = resourceRule.RelatedRule.ModifiedAt,
            Status = resourceRule.RelatedRule.Status,
            Priority = resourceRule.RelatedRule.Priority,
            RuleName = resourceRule.Rule.Name,
            RuleExpression = resourceRule.Rule.Expression
        };
    }

    public async Task AddRuleAsync(Guid resourceId, AddRuleToResourceInput input,
        CancellationToken cancellationToken = default)
    {
        var resource = await resourceRepository.GetWithRuleAsync(resourceId, cancellationToken);
        resource.AddRule(input.RuleId, input.ClientId, input.Priority);
        await resourceRepository.UpdateAsync(resource, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateRuleAsync(Guid resourceId, Guid ruleId, UpdateResourceRuleInput input,
        CancellationToken cancellationToken = default)
    {
        var resource = await resourceRepository.GetWithRuleAsync(resourceId, cancellationToken);
        resource.UpdateRule(ruleId, input.Status, input.Priority);
        await resourceRepository.UpdateAsync(resource, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveRuleAsync(Guid resourceId, Guid ruleId, CancellationToken cancellationToken = default)
    {
        var resource = await resourceRepository.GetWithRuleAsync(resourceId, cancellationToken);
        resource.RemoveRule(ruleId);
        await resourceRepository.UpdateAsync(resource, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<ListResultDto<ResourcePrivilegeDto>> GetPrivilegesAsync(Guid resourceId,
        CancellationToken cancellationToken = default)
    {
        var resourcePrivileges = await resourceRepository.GetPrivilegesAsync(resourceId, cancellationToken);
        var items = resourcePrivileges.Select(resourceRule =>
            new ResourcePrivilegeDto()
            {
                PrivilegeId = resourceRule.RelatedPrivilege.PrivilegeId,
                ClientId = resourceRule.RelatedPrivilege.ClientId,
                Status = resourceRule.RelatedPrivilege.Status,
                Priority = resourceRule.RelatedPrivilege.Priority,
                CreatedAt = resourceRule.RelatedPrivilege.CreatedAt,
                ModifiedAt = resourceRule.RelatedPrivilege.ModifiedAt,
                PrivilegeUrl = resourceRule.Privilege.Url,
                PrivilegeType = resourceRule.Privilege.Type
            }).ToList();
        return new ListResultDto<ResourcePrivilegeDto>(items);
    }

    public async Task<ResourcePrivilegeDto> GetPrivilegeAsync(Guid resourceId, Guid privilegeId,
        CancellationToken cancellationToken = default)
    {
        var resourcePrivilege = await resourceRepository.GetPrivilegeAsync(resourceId, privilegeId, cancellationToken);
        return new ResourcePrivilegeDto()
        {
            PrivilegeId = resourcePrivilege.RelatedPrivilege.PrivilegeId,
            ClientId = resourcePrivilege.RelatedPrivilege.ClientId,
            CreatedAt = resourcePrivilege.RelatedPrivilege.CreatedAt,
            ModifiedAt = resourcePrivilege.RelatedPrivilege.ModifiedAt,
            Status = resourcePrivilege.RelatedPrivilege.Status,
            Priority = resourcePrivilege.RelatedPrivilege.Priority,
            PrivilegeUrl = resourcePrivilege.Privilege.Url,
            PrivilegeType = resourcePrivilege.Privilege.Type
        };
    }

    public async Task AddPrivilegeAsync(Guid resourceId, AddPrivilegeToResourceInput input,
        CancellationToken cancellationToken = default)
    {
        var resource = await resourceRepository.GetWithRuleAsync(resourceId, cancellationToken);
        resource.AddPrivilege(input.PrivilegeId, input.ClientId, input.Priority);
        await resourceRepository.UpdateAsync(resource, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdatePrivilegeAsync(Guid resourceId, Guid privilegeId, UpdateResourcePrivilegeInput input,
        CancellationToken cancellationToken = default)
    {
        var resource = await resourceRepository.GetWithPrivilegeAsync(resourceId, cancellationToken);
        resource.UpdatePrivilege(privilegeId, input.Status, input.Priority);
        await resourceRepository.UpdateAsync(resource, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task RemovePrivilegeAsync(Guid resourceId, Guid privilegeId,
        CancellationToken cancellationToken = default)
    {
        var resource = await resourceRepository.GetWithRuleAsync(resourceId, cancellationToken);
        resource.RemovePrivilege(privilegeId);
        await resourceRepository.UpdateAsync(resource, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<ListResultDto<ResourcePolicyDto>> GetPoliciesAsync(Guid resourceId,
        CancellationToken cancellationToken = default)
    {
        var resourcePolicies = await resourceRepository.GetPoliciesAsync(resourceId, cancellationToken);
        var items = resourcePolicies.Select(item =>
            new ResourcePolicyDto()
            {
                CreatedAt = item.RelatedPolicy.CreatedAt,
                ModifiedAt = item.RelatedPolicy.ModifiedAt,
                Status = item.RelatedPolicy.Status,
                Priority = item.RelatedPolicy.Priority,
                Clients = item.RelatedPolicy.Clients,
                PolicyName = item.Policy.Name,
                Effect = item.Policy.Effect
            }).ToList();
        return new ListResultDto<ResourcePolicyDto>(items);
    }

    public async Task<ResourcePolicyDto> GetPolicyAsync(Guid resourceId, Guid policyId,
        CancellationToken cancellationToken = default)
    {
        var resourcePrivilege = await resourceRepository.GetPolicyAsync(resourceId, policyId, cancellationToken);
        return new ResourcePolicyDto()
        {
            CreatedAt = resourcePrivilege.RelatedPolicy.CreatedAt,
            ModifiedAt = resourcePrivilege.RelatedPolicy.ModifiedAt,
            Status = resourcePrivilege.RelatedPolicy.Status,
            Priority = resourcePrivilege.RelatedPolicy.Priority,
            Clients = resourcePrivilege.RelatedPolicy.Clients,
            PolicyName = resourcePrivilege.Policy.Name,
            Effect = resourcePrivilege.Policy.Effect
        };
    }

    public async Task AddPolicyAsync(Guid resourceId, AddPolicyToResourceInput input, CancellationToken cancellationToken = default)
    {
        var resource = await resourceRepository.GetWithPoliciesAsync(resourceId, cancellationToken);
        resource.AddPolicy(input.PolicyId, input.Clients, input.Priority);
        await resourceRepository.UpdateAsync(resource, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdatePolicyAsync(Guid resourceId, Guid policyId, UpdateResourcePolicyInput input,
        CancellationToken cancellationToken = default)
    {
        var resource = await resourceRepository.GetWithPoliciesAsync(resourceId, cancellationToken);
        resource.UpdatePolicy(policyId, input.Clients, input.Status, input.Priority);
        await resourceRepository.UpdateAsync(resource, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task RemovePolicyAsync(Guid resourceId, Guid policyId, CancellationToken cancellationToken = default)
    {
        var resource = await resourceRepository.GetWithPoliciesAsync(resourceId, cancellationToken);
        resource.RemovePolicy(policyId);
        await resourceRepository.UpdateAsync(resource, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task MapAsync(ResourceRuleMapInput input, CancellationToken cancellationToken = default)
    {
        var ruleNames = input.RuleName.Split(",");

        var mapResults = await resourceManager.MapAsync(
            input.Url,
            ruleNames,
            input.GetResourceTypes(),
            cancellationToken);

        var resourcesToBeAdded = mapResults.Where(p => !p.Exists)
            .Select(s => s.Resource).ToList();
        var resourcesToBeUpdated = mapResults.Where(p => p.Exists)
            .Select(s => s.Resource).ToList();

        if (resourcesToBeAdded.Any())
        {
            await resourceRepository.InsertManyAsync(resourcesToBeAdded, cancellationToken);
        }

        if (resourcesToBeUpdated.Any())
        {
            await resourceRepository.UpdateManyAsync(resourcesToBeUpdated, cancellationToken);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
