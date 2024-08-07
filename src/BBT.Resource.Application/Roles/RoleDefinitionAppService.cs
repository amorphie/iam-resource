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

public class RoleDefinitionAppService(
    IServiceProvider serviceProvider,
    IUnitOfWork unitOfWork,
    IRoleDefinitionRepository roleDefinitionRepository,
    MultiLingualRoleDefinitionObjectMapper multiLingualMapper)
    : ApplicationService(serviceProvider), IRoleDefinitionAppService
{
    public async Task<PagedResultDto<RoleDefinitionMultiLingualDto>> GetAllAsync(PagedRoleDefinitionInput input,
        CancellationToken cancellationToken = default)
    {
        var totalCount = await roleDefinitionRepository.LongCountAsync(cancellationToken);
        var items = await roleDefinitionRepository.GetPagedListAsync(input.SkipCount, input.MaxResultCount,
            input.Sorting, true, cancellationToken);

        var roleDefDtos = items.Select(multiLingualMapper.Map)
            .ToList();

        return new PagedResultDto<RoleDefinitionMultiLingualDto>
        {
            TotalCount = totalCount,
            Items = roleDefDtos
        };
    }

    public async Task<RoleDefinitionMultiLingualDto> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var roleDefinition = await roleDefinitionRepository.GetAsync(id, true, cancellationToken);
        return multiLingualMapper.Map(roleDefinition);
    }

    public async Task<RoleDefinitionDto> CreateAsync(CreateRoleDefinitionInput input,
        CancellationToken cancellationToken = default)
    {
        var roleDefinition = new RoleDefinition(
            input.Id,
            input.Key,
            input.ClientId)
        {
            Tags = input.Tags
        };

        foreach (var translation in input.Translations)
        {
            roleDefinition.AddTranslation(translation.Language, translation.Name, translation.Description);
        }

        await roleDefinitionRepository.InsertAsync(roleDefinition, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return ObjectMapper.Map<RoleDefinition, RoleDefinitionDto>(roleDefinition);
    }

    public async Task<RoleDefinitionDto> UpdateAsync(Guid id, UpdateRoleDefinitionInput input,
        CancellationToken cancellationToken = default)
    {
        var roleDefinition = await roleDefinitionRepository.GetAsync(id, true, cancellationToken);
        roleDefinition.SetKey(input.Key);
        roleDefinition.Tags = input.Tags;
        roleDefinition.ChangeStatus(input.Status);

        foreach (var translation in input.Translations)
        {
            roleDefinition.AddTranslation(translation.Language, translation.Name, translation.Description);
        }

        var translationsToRemove = new List<RoleDefinitionTranslation>();
        foreach (var translation in roleDefinition.Translations)
        {
            if (!input.Translations.Exists(a => a.Language == translation.Language))
            {
                translationsToRemove.Add(translation);
            }
        }

        foreach (var translation in translationsToRemove)
        {
            roleDefinition.RemoveTranslation(translation);
        }

        await roleDefinitionRepository.UpdateAsync(roleDefinition, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return ObjectMapper.Map<RoleDefinition, RoleDefinitionDto>(roleDefinition);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await roleDefinitionRepository.DeleteAsync(id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
