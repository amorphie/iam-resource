using System;
using System.Collections.Generic;
using BBT.Prism.Application.Dtos;
using BBT.Prism.Auditing;

namespace BBT.Resource.Roles;

public class RoleGroupDto(List<RoleGroupTranslationDto> translations) : EntityDto<Guid>, IHasCreatedAt, IHasModifyTime
{
    public string[]? Tags { get; set; }
    public Status Status { get; set; }
    public List<RoleGroupTranslationDto> Translations { get; set; } = translations;
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
