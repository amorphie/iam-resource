using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BBT.Resource.Roles;

public class UpdateRoleGroupInput(List<RoleGroupTranslationDto> translations)
{
    public string[]? Tags { get; set; }

    [Required]
    [MaxLength(SharedConsts.MaxStatusLength)]
    public string Status { get; set; }

    public List<RoleGroupTranslationDto> Translations { get; set; } = translations;
}
