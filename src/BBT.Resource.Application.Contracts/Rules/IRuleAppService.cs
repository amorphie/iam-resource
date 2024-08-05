using System;
using System.Threading;
using System.Threading.Tasks;
using BBT.Prism.Application.Dtos;
using BBT.Prism.Application.Services;

namespace BBT.Resource.Rules;

public interface IRuleAppService : IApplicationService
{
    Task<PagedResultDto<RuleDto>> GetAllAsync(PagedRuleInput input, CancellationToken cancellationToken = default);
    Task<RuleDto> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<RuleDto> CreateAsync(CreateRuleInput input, CancellationToken cancellationToken = default);
    Task<RuleDto> UpdateAsync(Guid id, UpdateRuleInput input, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
