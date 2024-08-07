using BBT.Prism.Domain.Services;
using BBT.Prism.Threading;

namespace BBT.Resource.Resources;

public class MultiLingualResourceObjectMapper(IMultiLingualEntityManager multiLingualManager)
{
    public ResourceMultiLingualDto Map(Resource source)
    {
        var translation = AsyncHelper.RunSync(() =>
            multiLingualManager.GetTranslationAsync<Resource, ResourceTranslation>(source));

        return new ResourceMultiLingualDto()
        {
            Id = source.Id,
            Name = translation?.Name ?? "",
            Description = translation?.Description,
            GroupId = source.GroupId,
            Url = source.Url,
            Type = source.Type,
            Tags = source.Tags,
            Status = source.Status,
            CreatedAt = source.CreatedAt,
            ModifiedAt = source.CreatedAt
        };
    }
}
