using System;
using System.Collections.Generic;
using BBT.Prism.Application.Dtos;

namespace BBT.Resource.Roles;

public class CreateRoleGroupInput : EntityDto<Guid>
{
    public string[]? Tags { get; set; }
    public List<RoleGroupTranslationDto> Translations { get; set; }
}
