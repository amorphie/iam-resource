using BBT.Prism.Domain.Services;
using BBT.Prism.Threading;

namespace BBT.Resource.Resources;

public sealed class MultiLingualResourceGroupObjectMapper(IMultiLingualEntityManager multiLingualManager)
{
    public ResourceGroupMultiLingualDto Map(ResourceGroup source)
    {
        var translation = AsyncHelper.RunSync(() =>
            multiLingualManager.GetTranslationAsync<ResourceGroup, ResourceGroupTranslation>(source));

        return new ResourceGroupMultiLingualDto()
        {
            Id = source.Id,
            Name = translation?.Name ?? "",
            Tags = source.Tags,
            Status = source.Status,
            CreatedAt = source.CreatedAt,
            ModifiedAt = source.CreatedAt
        };
    }
}
