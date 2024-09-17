using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BBT.Prism.Application.Dtos;
using BBT.Prism.Application.Services;
using BBT.Prism.Domain.Repositories;
using BBT.Prism.Uow;
using BBT.Resource.PolicyEngine.ConflictResolutions.Options;
using BBT.Resource.PolicyEngine.Evaluations.Options;
using Microsoft.Extensions.Options;

namespace BBT.Resource.Policies;

public class PoliciesAppService(
    IServiceProvider serviceProvider,
    IUnitOfWork unitOfWork,
    IPolicyRepository policyRepository,
    IOptions<EvaluationOptions> evaluationOptions,
    IOptions<ConflictResolutionOptions> conflictResolutionOptions)
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

        policy.SetTemplate(input.Template);
        policy.SetPermissions(input.PermissionProvider, input.Permissions);
        policy.SetEvaluationOrder(input.EvaluationOrder);

        PolicyTime? time = null;

        if (input.Condition?.Time != null)
        {
            time = new PolicyTime(input.Condition.Time.Start, input.Condition.Time.End, input.Condition.Time.Timezone);
        }

        policy.UpdateCondition(
            input.Condition?.Context,
            input.Condition?.Attributes,
            input.Condition?.Roles,
            input.Condition?.Rules,
            time);

        await policyRepository.InsertAsync(policy, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return ObjectMapper.Map<Policy, PolicyDto>(policy);
    }

    public async Task<PolicyDto> UpdateAsync(Guid id, UpdatePolicyInput input,
        CancellationToken cancellationToken = default)
    {
        var policy = await policyRepository.GetAsync(id, true, cancellationToken);
        policy.SetTemplate(input.Template);
        policy.SetName(input.Name);
        policy.SetPriority(input.Priority);
        policy.ParentId = input.ParentId;
        policy.SetPermissions(input.PermissionProvider, input.Permissions);
        policy.SetEvaluationOrder(input.EvaluationOrder);

        PolicyTime? time = null;

        if (input.Condition?.Time != null)
        {
            time = new PolicyTime(input.Condition.Time.Start, input.Condition.Time.End, input.Condition.Time.Timezone);
        }

        policy.UpdateCondition(
            input.Condition?.Context,
            input.Condition?.Attributes,
            input.Condition?.Roles,
            input.Condition?.Rules,
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

    public Task<ListResultDto<EvaluationStepInfoDto>> GetEvaluationStepsAsync(
        CancellationToken cancellationToken = default)
    {
        var list = evaluationOptions.Value.Steps.Select(s => new EvaluationStepInfoDto()
        {
            Name = s.Key,
            DisplayName = s.Value.DisplayName
        }).ToList();
        return Task.FromResult(new ListResultDto<EvaluationStepInfoDto>(list));
    }

    public Task<ListResultDto<ConflictResolutionInfoDto>> GetConflictResolutionsAsync(
        CancellationToken cancellationToken = default)
    {
        var list = conflictResolutionOptions.Value.Strategies.Select(s => new ConflictResolutionInfoDto()
        {
            Name = s.Key,
            DisplayName = s.Value.DisplayName,
            Description = s.Value.Description
        }).ToList();
        return Task.FromResult(new ListResultDto<ConflictResolutionInfoDto>(list));
    }
}
