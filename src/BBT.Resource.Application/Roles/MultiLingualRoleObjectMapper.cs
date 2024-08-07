using BBT.Prism.Domain.Services;
using BBT.Prism.Threading;

namespace BBT.Resource.Roles;

public sealed class MultiLingualRoleObjectMapper(IMultiLingualEntityManager multiLingualManager)
{
    public RoleListMultiLingualDto Map(Role source)
    {
        var translation = AsyncHelper.RunSync(() =>
            multiLingualManager.GetTranslationAsync<Role, RoleTranslation>(source));

        return new RoleListMultiLingualDto()
        {
            Id = source.Id,
            DefinitionId = source.DefinitionId,
            Name = translation?.Name ?? "",
            Tags = source.Tags,
            Status = source.Status,
            CreatedAt = source.CreatedAt,
            ModifiedAt = source.CreatedAt
        };
    }

    public RoleMultiLingualDto Map(RoleWithDefinitionModel source)
    {
        var translation = AsyncHelper.RunSync(() =>
            multiLingualManager.GetTranslationAsync<Role, RoleTranslation>(source.Role));

        var translationDef = AsyncHelper.RunSync(() =>
            multiLingualManager.GetTranslationAsync<RoleDefinition, RoleDefinitionTranslation>(source.Definition));

        return new RoleMultiLingualDto()
        {
            Id = source.Role.Id,
            DefinitionId = source.Role.DefinitionId,
            DefinitionName = translationDef?.Name ?? "",
            Name = translation?.Name ?? "",
            Tags = source.Role.Tags,
            Status = source.Role.Status,
            CreatedAt = source.Role.CreatedAt,
            ModifiedAt = source.Role.CreatedAt
        };
    }
}
