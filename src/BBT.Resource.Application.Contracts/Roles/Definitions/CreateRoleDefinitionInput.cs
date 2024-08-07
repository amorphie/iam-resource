using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BBT.Prism.Application.Dtos;

namespace BBT.Resource.Roles;

public class CreateRoleDefinitionInput(List<RoleDefinitionTranslationDto> translations) : EntityDto<Guid>
{
    [Required]
    [MaxLength(RoleDefinitionConsts.MaxKeyLength)]
    public string Key { get; set; }
    [Required]
    public Guid ClientId { get; set; }
    public string[]? Tags { get; set; }
    public List<RoleDefinitionTranslationDto> Translations { get; set; } = translations;
}
