using System;
using System.Threading;
using System.Threading.Tasks;
using BBT.Prism.Application.Dtos;
using BBT.Prism.Application.Services;

namespace BBT.Resource.Resources;

public interface IResourceAppService : IApplicationService
{
    Task<PagedResultDto<ResourceMultiLingualDto>> GetAllAsync(PagedResourceInput input,
        CancellationToken cancellationToken = default);

    Task<ResourceMultiLingualDto> GetAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ResourceDto> CreateAsync(CreateResourceInput input, CancellationToken cancellationToken = default);

    Task<ResourceDto> UpdateAsync(Guid id, UpdateResourceInput input,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ListResultDto<ResourceRuleDto>> GetRulesAsync(Guid resourceId, CancellationToken cancellationToken = default);

    Task<ResourceRuleDto> GetRuleAsync(Guid resourceId, Guid ruleId, CancellationToken cancellationToken = default);

    Task AddRuleAsync(Guid resourceId, AddRuleToResourceInput input, CancellationToken cancellationToken = default);

    Task UpdateRuleAsync(Guid resourceId, Guid ruleId, UpdateResourceRuleInput input,
        CancellationToken cancellationToken = default);

    Task RemoveRuleAsync(Guid resourceId, Guid ruleId, CancellationToken cancellationToken = default);

    Task<ListResultDto<ResourcePrivilegeDto>> GetPrivilegesAsync(Guid resourceId,
        CancellationToken cancellationToken = default);

    Task<ResourcePrivilegeDto> GetPrivilegeAsync(Guid resourceId, Guid privilegeId,
        CancellationToken cancellationToken = default);

    Task AddPrivilegeAsync(Guid resourceId, AddPrivilegeToResourceInput input,
        CancellationToken cancellationToken = default);

    Task UpdatePrivilegeAsync(Guid resourceId, Guid privilegeId, UpdateResourcePrivilegeInput input,
        CancellationToken cancellationToken = default);

    Task RemovePrivilegeAsync(Guid resourceId, Guid privilegeId, CancellationToken cancellationToken = default);
    
    Task<ListResultDto<ResourcePolicyDto>> GetPoliciesAsync(Guid resourceId,
        CancellationToken cancellationToken = default);
    
    Task<ResourcePolicyDto> GetPolicyAsync(Guid resourceId, Guid policyId,
        CancellationToken cancellationToken = default);
    
    Task AddPolicyAsync(Guid resourceId, AddPolicyToResourceInput input,
        CancellationToken cancellationToken = default);

    Task UpdatePolicyAsync(Guid resourceId, Guid policyId, UpdateResourcePolicyInput input,
        CancellationToken cancellationToken = default);

    Task RemovePolicyAsync(Guid resourceId, Guid policyId, CancellationToken cancellationToken = default);

    Task MapAsync(ResourceRuleMapInput input, CancellationToken cancellationToken = default);
}
