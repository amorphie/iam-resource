using System.ComponentModel.DataAnnotations;
using BBT.Prism.MultiLingualEntities;

namespace BBT.Resource.Roles;

public class RoleDefinitionTranslationDto : IEntityTranslation
{
    [Required]
    [MaxLength(RoleDefinitionConsts.MaxNameLength)]
    public string Name { get; set; }

    [MaxLength(RoleDefinitionConsts.MaxDescriptionLength)]
    public string? Description { get; set; }

    [Required]
    [MaxLength(SharedConsts.MaxLanguageLength)]
    public string Language { get; set; }
}
