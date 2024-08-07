using BBT.Prism.Domain.Services;
using BBT.Prism.Threading;

namespace BBT.Resource.Roles;

public sealed class MultiLingualRoleDefinitionObjectMapper(IMultiLingualEntityManager multiLingualManager)
{
    public RoleDefinitionMultiLingualDto Map(RoleDefinition source)
    {
        var translation = AsyncHelper.RunSync(() =>
            multiLingualManager.GetTranslationAsync<RoleDefinition, RoleDefinitionTranslation>(source));

        return new RoleDefinitionMultiLingualDto()
        {
            Id = source.Id,
            Key = source.Key,
            ClientId = source.ClientId,
            Name = translation?.Name ?? "",
            Description = translation?.Description,
            Tags = source.Tags,
            Status = source.Status,
            CreatedAt = source.CreatedAt,
            ModifiedAt = source.CreatedAt
        };
    }
}
