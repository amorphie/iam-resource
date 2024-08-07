using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BBT.Prism.Application.Dtos;
using BBT.Prism.Application.Services;
using BBT.Prism.Domain.Repositories;
using BBT.Prism.Uow;

namespace BBT.Resource.Roles;

public class RoleGroupAppService(
    IServiceProvider serviceProvider,
    IUnitOfWork unitOfWork,
    IRoleGroupRepository roleGroupRepository,
    MultiLingualRoleGroupObjectMapper multiLingualMapper)
    : ApplicationService(serviceProvider), IRoleGroupAppService
{
    public async Task<PagedResultDto<RoleGroupMultiLingualDto>> GetAllAsync(PagedRoleGroupInput input,
        CancellationToken cancellationToken = default)
    {
        var totalCount = await roleGroupRepository.LongCountAsync(cancellationToken);
        var items = await roleGroupRepository.GetPagedListAsync(input.SkipCount, input.MaxResultCount,
            input.Sorting, true, cancellationToken);

        var roleDefDtos = items.Select(multiLingualMapper.Map)
            .ToList();

        return new PagedResultDto<RoleGroupMultiLingualDto>
        {
            TotalCount = totalCount,
            Items = roleDefDtos
        };
    }

    public async Task<RoleGroupMultiLingualDto> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var roleGroup = await roleGroupRepository.GetAsync(id, true, cancellationToken);
        return multiLingualMapper.Map(roleGroup);
    }

    public async Task<RoleGroupDto> CreateAsync(CreateRoleGroupInput input,
        CancellationToken cancellationToken = default)
    {
        var roleGroup = new RoleGroup(
            input.Id)
        {
            Tags = input.Tags
        };

        foreach (var translation in input.Translations)
        {
            roleGroup.AddTranslation(translation.Language, translation.Name);
        }

        await roleGroupRepository.InsertAsync(roleGroup, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return ObjectMapper.Map<RoleGroup, RoleGroupDto>(roleGroup);
    }

    public async Task<RoleGroupDto> UpdateAsync(Guid id, UpdateRoleGroupInput input,
        CancellationToken cancellationToken = default)
    {
        var roleGroup = await roleGroupRepository.GetAsync(id, true, cancellationToken);
        roleGroup.Tags = input.Tags;
        roleGroup.ChangeStatus(input.Status);

        foreach (var translation in input.Translations)
        {
            roleGroup.AddTranslation(translation.Language, translation.Name);
        }

        var translationsToRemove = new List<RoleGroupTranslation>();
        foreach (var translation in roleGroup.Translations)
        {
            if (!input.Translations.Exists(a => a.Language == translation.Language))
            {
                translationsToRemove.Add(translation);
            }
        }

        foreach (var translation in translationsToRemove)
        {
            roleGroup.RemoveTranslation(translation);
        }

        await roleGroupRepository.UpdateAsync(roleGroup, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return ObjectMapper.Map<RoleGroup, RoleGroupDto>(roleGroup);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await roleGroupRepository.DeleteAsync(id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<ListResultDto<RoleGroupRoleDto>> GetRolesAsync(Guid groupId,
        CancellationToken cancellationToken = default)
    {
        var groupRelatedRoles = await roleGroupRepository.GetRolesAsync(groupId, cancellationToken);
        return new ListResultDto<RoleGroupRoleDto>(
            groupRelatedRoles.Select(multiLingualMapper.Map).ToList()
        );
    }

    public async Task<RoleGroupRoleDto> GetRoleAsync(Guid groupId, Guid roleId,
        CancellationToken cancellationToken = default)
    {
        var groupRelatedRole = await roleGroupRepository.GetRoleAsync(groupId, roleId, cancellationToken);
        return multiLingualMapper.Map(groupRelatedRole);
    }

    public async Task AddRoleAsync(Guid groupId, AddRoleToRoleGroupInput input,
        CancellationToken cancellationToken = default)
    {
        var roleGroup = await roleGroupRepository.GetAsync(groupId, true, cancellationToken);
        roleGroup.AddRole(input.RoleId);
        await roleGroupRepository.UpdateAsync(roleGroup, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task ChangeRoleStatusAsync(Guid groupId, Guid roleId, ChangeStatusOfGroupRoleInput input,
        CancellationToken cancellationToken = default)
    {
        var roleGroup = await roleGroupRepository.GetAsync(groupId, true, cancellationToken);
        roleGroup.ChangeRoleStatus(roleId, input.Status);
        await roleGroupRepository.UpdateAsync(roleGroup, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveRoleAsync(Guid groupId, Guid roleId, CancellationToken cancellationToken = default)
    {
        var roleGroup = await roleGroupRepository.GetAsync(groupId, true, cancellationToken);
        roleGroup.RemoveRole(roleId);
        await roleGroupRepository.UpdateAsync(roleGroup, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
