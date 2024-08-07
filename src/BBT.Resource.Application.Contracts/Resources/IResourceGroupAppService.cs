using System;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using BBT.Prism.Application.Dtos;
using BBT.Prism.Application.Services;

namespace BBT.Resource.Resources;

public interface IResourceGroupAppService : IApplicationService
{
    Task<PagedResultDto<ResourceGroupMultiLingualDto>> GetAllAsync(PagedResourceGroupInput input,
        CancellationToken cancellationToken = default);

    Task<ResourceGroupMultiLingualDto> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ResourceGroupDto> CreateAsync(CreateResourceGroupInput input, CancellationToken cancellationToken = default);

    Task<ResourceGroupDto> UpdateAsync(Guid id, UpdateResourceGroupInput input,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
