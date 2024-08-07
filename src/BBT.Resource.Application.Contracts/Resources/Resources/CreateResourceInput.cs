using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BBT.Prism.Application.Dtos;

namespace BBT.Resource.Resources;

public class CreateResourceInput : EntityDto<Guid>
{
    public Guid? GroupId { get; set; }
    [Required] public ResourceType Type { get; set; }

    [Required]
    [MaxLength(ResourceConsts.MaxUrlLength)]
    public string Url { get; set; }

    public string[]? Tags { get; set; }

    public List<ResourceTranslationDto> Translations { get; set; }
}
