using System;
using BBT.Prism.Application.Dtos;

namespace BBT.Resource.Roles;

public class RoleGroupDto : EntityDto<Guid>
{
    public string[]? Tags { get; set; }
    public Status Status { get; set; }
}
