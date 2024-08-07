using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BBT.Prism.Application.Dtos;
using BBT.Prism.Application.Services;
using BBT.Prism.Domain.Repositories;
using BBT.Prism.Uow;

namespace BBT.Resource.Resources;

public class ResourceGroupAppService(
    IServiceProvider serviceProvider,
    IUnitOfWork unitOfWork,
    IResourceGroupRepository resourceGroupRepository,
    MultiLingualResourceGroupObjectMapper multiLingualMapper)
    : ApplicationService(serviceProvider), IResourceGroupAppService
{
    public async Task<PagedResultDto<ResourceGroupMultiLingualDto>> GetAllAsync(PagedResourceGroupInput input,
        CancellationToken cancellationToken = default)
    {
        var totalCount = await resourceGroupRepository.LongCountAsync(cancellationToken);
        var items = await resourceGroupRepository.GetPagedListAsync(input.SkipCount, input.MaxResultCount,
            input.Sorting, true, cancellationToken);

        var roleDefDtos = items.Select(multiLingualMapper.Map)
            .ToList();

        return new PagedResultDto<ResourceGroupMultiLingualDto>
        {
            TotalCount = totalCount,
            Items = roleDefDtos
        };
    }

    public async Task<ResourceGroupMultiLingualDto> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var resourceGroup = await resourceGroupRepository.GetAsync(id, true, cancellationToken);
        return multiLingualMapper.Map(resourceGroup);
    }

    public async Task<ResourceGroupDto> CreateAsync(CreateResourceGroupInput input,
        CancellationToken cancellationToken = default)
    {
        var resourceGroup = new ResourceGroup(input.Id)
        {
            Tags = input.Tags
        };

        foreach (var translation in input.Translations)
        {
            resourceGroup.AddTranslation(translation.Language, translation.Name);
        }

        await resourceGroupRepository.InsertAsync(resourceGroup, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return ObjectMapper.Map<ResourceGroup, ResourceGroupDto>(resourceGroup);
    }

    public async Task<ResourceGroupDto> UpdateAsync(Guid id, UpdateResourceGroupInput input,
        CancellationToken cancellationToken = default)
    {
        var resourceGroup = await resourceGroupRepository.GetAsync(id, true, cancellationToken);
        resourceGroup.Tags = input.Tags;
        resourceGroup.ChangeStatus(input.Status);

        foreach (var translation in input.Translations)
        {
            resourceGroup.AddTranslation(translation.Language, translation.Name);
        }

        var translationsToRemove = new List<ResourceGroupTranslation>();
        foreach (var translation in resourceGroup.Translations)
        {
            if (!input.Translations.Exists(a => a.Language == translation.Language))
            {
                translationsToRemove.Add(translation);
            }
        }

        foreach (var translation in translationsToRemove)
        {
            resourceGroup.RemoveTranslation(translation);
        }

        await resourceGroupRepository.UpdateAsync(resourceGroup, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return ObjectMapper.Map<ResourceGroup, ResourceGroupDto>(resourceGroup);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await resourceGroupRepository.DeleteAsync(id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
