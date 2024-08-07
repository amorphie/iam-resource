using System;
using System.Collections.Generic;
using BBT.Prism.Application.Dtos;
using BBT.Prism.Auditing;

namespace BBT.Resource.Resources;

public class ResourceGroupDto : EntityDto<Guid>, IHasCreatedAt, IHasModifyTime
{
    public string[]? Tags { get; set; }
    public Status Status { get; set; }
    public List<ResourceGroupTranslationDto>? Translations { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
