using System;
using System.Threading;
using System.Threading.Tasks;
using BBT.Prism.Application.Dtos;
using BBT.Prism.Application.Services;
using BBT.Prism.Uow;

namespace BBT.Resource.Rules;

public class RuleAppService(
    IServiceProvider serviceProvider,
    IUnitOfWork unitOfWork,
    IRuleRepository ruleRepository)
    : ApplicationService(serviceProvider), IRuleAppService
{
    public Task<PagedResultDto<RuleDto>> GetAllAsync(PagedRuleInput input,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<RuleDto> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var rule = await ruleRepository.GetAsync(id, true, cancellationToken);
        return ObjectMapper.Map<Rule, RuleDto>(rule);
    }

    public async Task<RuleDto> CreateAsync(CreateRuleInput input, CancellationToken cancellationToken = default)
    {
        var rule = new Rule(input.Id, input.Name, input.Expression);
        await ruleRepository.InsertAsync(rule, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return ObjectMapper.Map<Rule, RuleDto>(rule);
    }

    public async Task<RuleDto> UpdateAsync(Guid id, UpdateRuleInput input, CancellationToken cancellationToken = default)
    {
        var rule = await ruleRepository.GetAsync(id, true, cancellationToken);
        rule.SetName(input.Name);
        rule.SetExpression(input.Expression);
        await ruleRepository.UpdateAsync(rule, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return ObjectMapper.Map<Rule, RuleDto>(rule);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await ruleRepository.DeleteAsync(id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
