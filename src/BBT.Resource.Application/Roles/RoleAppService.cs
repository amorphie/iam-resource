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

public class RoleAppService(
    IServiceProvider serviceProvider,
    IUnitOfWork unitOfWork,
    IRoleRepository roleRepository,
    MultiLingualRoleObjectMapper multiLingualMapper) : ApplicationService(serviceProvider),
    IRoleAppService
{
    public async Task<PagedResultDto<RoleListMultiLingualDto>> GetAllAsync(PagedRoleInput input,
        CancellationToken cancellationToken = default)
    {
        var totalCount = await roleRepository.LongCountAsync(cancellationToken);
        var items = await roleRepository.GetPagedListAsync(input.SkipCount, input.MaxResultCount,
            input.Sorting, true, cancellationToken);

        var roleDefDtos = items.Select(multiLingualMapper.Map)
            .ToList();

        return new PagedResultDto<RoleListMultiLingualDto>
        {
            TotalCount = totalCount,
            Items = roleDefDtos
        };
    }

    public async Task<RoleMultiLingualDto> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var role = await roleRepository.GetWithDefinitionAsync(id, cancellationToken);
        return multiLingualMapper.Map(role);
    }

    public async Task<RoleDto> CreateAsync(CreateRoleInput input,
        CancellationToken cancellationToken = default)
    {
        var role = new Role(
            input.Id,
            input.DefinitionId)
        {
            Tags = input.Tags
        };

        foreach (var translation in input.Translations)
        {
            role.AddTranslation(translation.Language, translation.Name);
        }

        await roleRepository.InsertAsync(role, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return ObjectMapper.Map<Role, RoleDto>(role);
    }

    public async Task<RoleDto> UpdateAsync(Guid id, UpdateRoleInput input,
        CancellationToken cancellationToken = default)
    {
        var role = await roleRepository.GetAsync(id, true, cancellationToken);
        role.Tags = input.Tags;
        role.ChangeStatus(input.Status);

        foreach (var translation in input.Translations)
        {
            role.AddTranslation(translation.Language, translation.Name);
        }

        var translationsToRemove = new List<RoleTranslation>();
        foreach (var translation in role.Translations)
        {
            if (!input.Translations.Exists(a => a.Language == translation.Language))
            {
                translationsToRemove.Add(translation);
            }
        }

        foreach (var translation in translationsToRemove)
        {
            role.RemoveTranslation(translation);
        }

        await roleRepository.UpdateAsync(role, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return ObjectMapper.Map<Role, RoleDto>(role);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await roleRepository.DeleteAsync(id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
