using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BBT.Resource.Resources;

public class UpdateResourceGroupInput(List<ResourceGroupTranslationDto> translations)
{
    public string[]? Tags { get; set; }

    [Required]
    [MaxLength(SharedConsts.MaxStatusLength)]
    public Status Status { get; set; }

    public List<ResourceGroupTranslationDto> Translations { get; set; } = translations;
}
