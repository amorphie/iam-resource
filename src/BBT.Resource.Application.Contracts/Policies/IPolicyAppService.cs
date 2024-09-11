using System;
using System.Threading;
using System.Threading.Tasks;
using BBT.Prism.Application.Dtos;
using BBT.Prism.Application.Services;

namespace BBT.Resource.Policies;

public interface IPolicyAppService : IApplicationService
{
    Task<PagedResultDto<PolicyListDto>> GetAllAsync(PagedPolicyInput input,
        CancellationToken cancellationToken = default);

    Task<PolicyDto> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PolicyDto> CreateAsync(CreatePolicyInput input, CancellationToken cancellationToken = default);

    Task<PolicyDto> UpdateAsync(Guid id, UpdatePolicyInput input,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
