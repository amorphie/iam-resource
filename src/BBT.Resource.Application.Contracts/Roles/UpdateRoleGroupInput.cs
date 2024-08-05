using System.Collections.Generic;

namespace BBT.Resource.Roles;

public class UpdateRoleGroupInput
{
    public string[]? Tags { get; set; }
    public Status Status { get; set; }
    public List<RoleGroupTranslationDto> Translations { get; set; }
}
