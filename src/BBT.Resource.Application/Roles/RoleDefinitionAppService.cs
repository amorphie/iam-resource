using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using BBT.Prism.Application.Services;
using BBT.Prism.Uow;

namespace BBT.Resource.Roles;

public class RoleDefinitionAppService(
    IServiceProvider serviceProvider,
    IUnitOfWork unitOfWork,
    IRoleDefinitionRepository roleDefinitionRepository)
    : ApplicationService(serviceProvider), IRoleDefinitionAppService
{
    public Task<PagedResult<RoleDefinitionDto>> GetAllAsync(PagedRoleDefinitionInput input,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<RoleDefinitionDto> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var roleDefinition = await roleDefinitionRepository.GetAsync(id, true, cancellationToken);
        return ObjectMapper.Map<RoleDefinition, RoleDefinitionDto>(roleDefinition);
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
        roleDefinition.ClientId = input.ClientId;
        roleDefinition.Tags = input.Tags;
        roleDefinition.Status = input.Status;

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
