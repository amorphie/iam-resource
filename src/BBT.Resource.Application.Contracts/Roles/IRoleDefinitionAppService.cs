using System;
using System.Threading;
using System.Threading.Tasks;
using BBT.Prism.Application.Dtos;
using BBT.Prism.Application.Services;

namespace BBT.Resource.Roles;

public interface IRoleDefinitionAppService : IApplicationService
{
    Task<PagedResultDto<RoleDefinitionMultiLingualDto>> GetAllAsync(PagedRoleDefinitionInput input,
        CancellationToken cancellationToken = default);

    Task<RoleDefinitionMultiLingualDto> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<RoleDefinitionDto> CreateAsync(CreateRoleDefinitionInput input, CancellationToken cancellationToken = default);

    Task<RoleDefinitionDto> UpdateAsync(Guid id, UpdateRoleDefinitionInput input,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
