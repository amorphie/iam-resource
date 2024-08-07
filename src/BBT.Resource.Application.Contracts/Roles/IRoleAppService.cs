using System;
using System.Threading;
using System.Threading.Tasks;
using BBT.Prism.Application.Dtos;
using BBT.Prism.Application.Services;

namespace BBT.Resource.Roles;

public interface IRoleAppService : IApplicationService
{
    Task<PagedResultDto<RoleListMultiLingualDto>> GetAllAsync(PagedRoleInput input,
        CancellationToken cancellationToken = default);

    Task<RoleMultiLingualDto> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<RoleDto> CreateAsync(CreateRoleInput input, CancellationToken cancellationToken = default);

    Task<RoleDto> UpdateAsync(Guid id, UpdateRoleInput input,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
