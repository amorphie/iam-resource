using System;
using System.Threading;
using System.Threading.Tasks;
using BBT.Prism.Application.Dtos;
using BBT.Prism.Application.Services;

namespace BBT.Resource.Roles;

public interface IRoleGroupAppService : IApplicationService
{
    Task<PagedResultDto<RoleGroupMultiLingualDto>> GetAllAsync(PagedRoleGroupInput input,
        CancellationToken cancellationToken = default);

    Task<RoleGroupMultiLingualDto> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<RoleGroupDto> CreateAsync(CreateRoleGroupInput input, CancellationToken cancellationToken = default);

    Task<RoleGroupDto> UpdateAsync(Guid id, UpdateRoleGroupInput input,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ListResultDto<RoleGroupRoleDto>> GetRolesAsync(Guid groupId, CancellationToken cancellationToken = default);
    Task<RoleGroupRoleDto> GetRoleAsync(Guid groupId, Guid roleId, CancellationToken cancellationToken = default);

    Task AddRoleAsync(Guid groupId, AddRoleToRoleGroupInput input,
        CancellationToken cancellationToken = default);

    Task ChangeRoleStatusAsync(Guid groupId, Guid roleId, ChangeStatusOfGroupRoleInput input,
        CancellationToken cancellationToken = default);

    Task RemoveRoleAsync(Guid groupId, Guid roleId,
        CancellationToken cancellationToken = default);
}
