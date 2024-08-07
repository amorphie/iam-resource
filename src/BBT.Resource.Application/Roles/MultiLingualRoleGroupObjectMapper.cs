using BBT.Prism.Domain.Services;
using BBT.Prism.Threading;

namespace BBT.Resource.Roles;

public sealed class MultiLingualRoleGroupObjectMapper(IMultiLingualEntityManager multiLingualManager)
{
    public RoleGroupMultiLingualDto Map(RoleGroup source)
    {
        var translation = AsyncHelper.RunSync(() =>
            multiLingualManager.GetTranslationAsync<RoleGroup, RoleGroupTranslation>(source));

        return new RoleGroupMultiLingualDto()
        {
            Id = source.Id,
            Name = translation?.Name ?? "",
            Tags = source.Tags,
            Status = source.Status,
            CreatedAt = source.CreatedAt,
            ModifiedAt = source.CreatedAt
        };
    }
    
    public RoleGroupRoleDto Map(RoleGroupRelatedRoleModel source)
    {
        var translation = AsyncHelper.RunSync(() =>
            multiLingualManager.GetTranslationAsync<Role, RoleTranslation>(source.Role));


        return new RoleGroupRoleDto()
        {
            RoleName = translation?.Name ?? "",
            RoleId = source.RelatedRole.RoleId,
            Status = source.RelatedRole.Status,
            CreatedAt = source.RelatedRole.CreatedAt,
            ModifiedAt = source.RelatedRole.CreatedAt
        };
    }
}
