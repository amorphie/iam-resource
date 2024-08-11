using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BBT.Resource.Resources;

public class UpdateResourceInput(List<ResourceTranslationDto> translations)
{
    [Required] public ResourceType Type { get; set; }
    [Required]
    [MaxLength(ResourceConsts.MaxUrlLength)]
    public string Url { get; set; }
    public string[]? Tags { get; set; }

    [Required]
    [MaxLength(SharedConsts.MaxStatusLength)]
    public string Status { get; set; }
    public List<ResourceTranslationDto> Translations { get; set; } = translations;
}
