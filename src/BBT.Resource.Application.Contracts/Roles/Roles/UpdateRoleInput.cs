using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BBT.Resource.Roles;

public class UpdateRoleInput(List<RoleTranslationDto> translations)
{
    public string[]? Tags { get; set; }
    [Required]
    [MaxLength(SharedConsts.MaxStatusLength)]
    public string Status { get; set; }
    public List<RoleTranslationDto> Translations { get; set; } = translations;
}
