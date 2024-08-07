using System.ComponentModel.DataAnnotations;
using BBT.Prism.MultiLingualEntities;

namespace BBT.Resource.Roles;

public class RoleTranslationDto: IEntityTranslation
{
    [Required]
    [MaxLength(RoleConsts.MaxNameLength)]
    public string Name { get; set; }

    [Required]
    [MaxLength(SharedConsts.MaxLanguageLength)]
    public string Language { get; set; }
}
