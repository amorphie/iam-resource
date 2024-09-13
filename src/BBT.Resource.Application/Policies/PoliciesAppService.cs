using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BBT.Prism.Application.Dtos;
using BBT.Prism.Application.Services;
using BBT.Prism.Domain.Repositories;
using BBT.Prism.Uow;

namespace BBT.Resource.Policies;

/*
 * TODO: 
 *Execution true => policy deny ise 403
 *Execution false => policy deny ise 200 
 * 
 */

public class PoliciesAppService(
    IServiceProvider serviceProvider,
    IUnitOfWork unitOfWork,
    IPolicyRepository policyRepository)
    : ApplicationService(serviceProvider), IPolicyAppService
{
    public async Task<PagedResultDto<PolicyListDto>> GetAllAsync(PagedPolicyInput input,
        CancellationToken cancellationToken = default)
    {
        var totalCount = await policyRepository.LongCountAsync(cancellationToken);
        var items = await policyRepository.GetPagedListAsync(input.SkipCount, input.MaxResultCount,
            input.Sorting, true, cancellationToken);

        return new PagedResultDto<PolicyListDto>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<Policy>, List<PolicyListDto>>(items)
        };
    }

    public async Task<PolicyDto> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var policy = await policyRepository.GetAsync(id, true, cancellationToken);
        return ObjectMapper.Map<Policy, PolicyDto>(policy);
    }

    public async Task<PolicyDto> CreateAsync(CreatePolicyInput input, CancellationToken cancellationToken = default)
    {
        var policy = new Policy(
            input.Id,
            input.Name,
            Effect.FromCode(input.Effect),
            input.Priority)
        {
            ParentId = input.ParentId
        };

        policy.SetPermissions(input.Permissions);
        policy.SetEvaluationOrder(input.EvaluationOrder);

        PolicyTime? time = null;

        if (input.Condition.Time != null)
        {
            time = new PolicyTime(input.Condition.Time.Start, input.Condition.Time.End, input.Condition.Time.Timezone);
        }

        policy.UpdateCondition(
            input.Condition.Context,
            input.Condition.Attributes,
            input.Condition.Roles,
            input.Condition.Rules,
            time);

        await policyRepository.InsertAsync(policy, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return ObjectMapper.Map<Policy, PolicyDto>(policy);
    }

    public async Task<PolicyDto> UpdateAsync(Guid id, UpdatePolicyInput input, CancellationToken cancellationToken = default)
    {
        var policy = await policyRepository.GetAsync(id, true, cancellationToken);
        policy.SetName(input.Name);
        policy.SetPriority(input.Priority);
        policy.ParentId = input.ParentId;
        policy.SetPermissions(input.Permissions);
        policy.SetEvaluationOrder(input.EvaluationOrder);

        PolicyTime? time = null;

        if (input.Condition.Time != null)
        {
            time = new PolicyTime(input.Condition.Time.Start, input.Condition.Time.End, input.Condition.Time.Timezone);
        }

        policy.UpdateCondition(
            input.Condition.Context,
            input.Condition.Attributes,
            input.Condition.Roles,
            input.Condition.Rules,
            time);

        await policyRepository.UpdateAsync(policy, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return ObjectMapper.Map<Policy, PolicyDto>(policy);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await policyRepository.DeleteAsync(id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
