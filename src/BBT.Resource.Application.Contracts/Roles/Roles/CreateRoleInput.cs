using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BBT.Prism.Application.Dtos;

namespace BBT.Resource.Roles;

public class CreateRoleInput(List<RoleTranslationDto> translations) : EntityDto<Guid>
{
    [Required]
    public Guid DefinitionId { get; set; }
    public string[]? Tags { get; set; }
    public List<RoleTranslationDto> Translations { get; set; } = translations;
}
