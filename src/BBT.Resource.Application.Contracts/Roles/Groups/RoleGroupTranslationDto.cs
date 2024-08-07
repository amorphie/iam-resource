using System.ComponentModel.DataAnnotations;
using BBT.Prism.MultiLingualEntities;

namespace BBT.Resource.Roles;

public class RoleGroupTranslationDto : IEntityTranslation
{
    [Required]
    [MaxLength(SharedConsts.MaxLanguageLength)]
    public string Language { get; set; }

    [Required]
    [MaxLength(RoleGroupConsts.MaxNameLength)]
    public string Name { get; set; }
}
