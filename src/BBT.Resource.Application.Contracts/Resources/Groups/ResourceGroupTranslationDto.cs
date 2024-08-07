using System.ComponentModel.DataAnnotations;
using BBT.Prism.MultiLingualEntities;

namespace BBT.Resource.Resources;

public class ResourceGroupTranslationDto : IEntityTranslation
{
    [Required]
    [MaxLength(ResourceGroupConsts.MaxNameLength)]
    public string Name { get; set; }

    [Required]
    [MaxLength(SharedConsts.MaxLanguageLength)]
    public string Language { get; set; }
}
