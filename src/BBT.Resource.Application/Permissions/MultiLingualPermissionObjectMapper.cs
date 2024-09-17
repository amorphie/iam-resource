using BBT.Prism.Domain.Services;
using BBT.Prism.Threading;

namespace BBT.Resource.Permissions;

public class MultiLingualPermissionObjectMapper(IMultiLingualEntityManager multiLingualManager)
{
    public PermissionGrantDto Map(Permission source)
    {
        var translation = AsyncHelper.RunSync(() =>
            multiLingualManager.GetTranslationAsync<Permission, PermissionTranslation>(source));

        return new PermissionGrantDto()
        {
            Name = source.Name,
            DisplayName = translation?.DisplayName ?? "",
            Description = translation?.Description ?? "",
            IsGranted = false,
            ParentName = ""
        };
    }
}
