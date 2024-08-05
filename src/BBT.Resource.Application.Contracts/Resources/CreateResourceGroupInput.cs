using System;
using System.Collections.Generic;
using BBT.Prism.Application.Dtos;

namespace BBT.Resource.Resources;

public class CreateResourceGroupInput(List<ResourceGroupTranslationDto> translations) : EntityDto<Guid>
{
    public string[]? Tags { get; set; }
    public List<ResourceGroupTranslationDto> Translations { get; set; } = translations;
}
