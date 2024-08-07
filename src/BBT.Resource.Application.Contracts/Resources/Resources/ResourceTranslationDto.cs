using System.ComponentModel.DataAnnotations;
using BBT.Prism.MultiLingualEntities;

namespace BBT.Resource.Resources;

public class ResourceTranslationDto : IEntityTranslation
{
    [Required]
    [MaxLength(ResourceConsts.MaxNameLength)]
    public string Name { get; set; }

    [MaxLength(ResourceConsts.MaxDescriptionLength)]
    public string Description { get; set; }

    [Required]
    [MaxLength(SharedConsts.MaxLanguageLength)]
    public string Language { get; set; }
}
