using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BBT.Prism.Application.Dtos;
using BBT.Prism.Application.Services;
using BBT.Prism.Domain.Repositories;
using BBT.Prism.Uow;

namespace BBT.Resource.Privileges;

public class PrivilegeAppService(
    IServiceProvider serviceProvider,
    IUnitOfWork unitOfWork,
    IPrivilegeRepository privilegeRepository)
    : ApplicationService(serviceProvider), IPrivilegeAppService
{
    public async Task<PagedResultDto<PrivilegeDto>> GetAllAsync(PagedPrivilegeInput input,
        CancellationToken cancellationToken = default)
    {
        var totalCount = await privilegeRepository.LongCountAsync(cancellationToken);
        var items = await privilegeRepository.GetPagedListAsync(input.SkipCount, input.MaxResultCount,
            input.Sorting, true, cancellationToken);

        return new PagedResultDto<PrivilegeDto>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<Privilege>, List<PrivilegeDto>>(items)
        };
    }

    public async Task<PrivilegeDto> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var privilege = await privilegeRepository.GetAsync(id, true, cancellationToken);
        return ObjectMapper.Map<Privilege, PrivilegeDto>(privilege);
    }

    public async Task<PrivilegeDto> CreateAsync(CreatePrivilegeInput input,
        CancellationToken cancellationToken = default)
    {
        var privilege = new Privilege(input.Id, input.Url, input.Type);
        await privilegeRepository.InsertAsync(privilege, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return ObjectMapper.Map<Privilege, PrivilegeDto>(privilege);
    }

    public async Task<PrivilegeDto> UpdateAsync(Guid id, UpdatePrivilegeInput input,
        CancellationToken cancellationToken = default)
    {
        var privilege = await privilegeRepository.GetAsync(id, true, cancellationToken);
        privilege.SetUrl(input.Url);
        privilege.Type = input.Type;
        await privilegeRepository.UpdateAsync(privilege, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return ObjectMapper.Map<Privilege, PrivilegeDto>(privilege);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await privilegeRepository.DeleteAsync(id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
