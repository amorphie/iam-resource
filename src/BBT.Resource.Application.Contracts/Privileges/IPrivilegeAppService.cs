using System;
using System.Threading;
using System.Threading.Tasks;
using BBT.Prism.Application.Dtos;
using BBT.Prism.Application.Services;

namespace BBT.Resource.Privileges;

public interface IPrivilegeAppService : IApplicationService
{
    Task<PagedResultDto<PrivilegeDto>> GetAllAsync(PagedPrivilegeInput input, CancellationToken cancellationToken = default);
    Task<PrivilegeDto> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PrivilegeDto> CreateAsync(CreatePrivilegeInput input, CancellationToken cancellationToken = default);
    Task<PrivilegeDto> UpdateAsync(Guid id, UpdatePrivilegeInput input, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
