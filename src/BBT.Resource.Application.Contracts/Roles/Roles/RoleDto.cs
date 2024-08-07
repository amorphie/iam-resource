using System;
using System.Collections.Generic;
using BBT.Prism.Application.Dtos;
using BBT.Prism.Auditing;

namespace BBT.Resource.Roles;

public class RoleDto(List<RoleTranslationDto> translations) : EntityDto<Guid>, IHasCreatedAt, IHasModifyTime
{
    public Guid DefinitionId { get; set; }
    public string[]? Tags { get; set; }
    public Status Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public List<RoleTranslationDto> Translations { get; set; } = translations;
}
