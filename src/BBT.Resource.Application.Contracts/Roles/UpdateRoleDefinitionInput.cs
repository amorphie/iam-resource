using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BBT.Resource.Roles;

public class UpdateRoleDefinitionInput(List<RoleDefinitionTranslationDto> translations)
{
    [Required]
    [MaxLength(RoleDefinitionConsts.MaxKeyLength)]
    public string Key { get; set; }
    [Required]
    public Guid ClientId { get; set; }
    public string[]? Tags { get; set; }
    public Status Status { get; set; }
    public List<RoleDefinitionTranslationDto> Translations { get; set; } = translations;
}
