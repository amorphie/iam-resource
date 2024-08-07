using System;
using BBT.Prism.Auditing;

namespace BBT.Resource.Roles;

public class RoleGroupRoleDto : IHasCreatedAt, IHasModifyTime
{
    public string RoleName { get; set; }
    public Guid RoleId { get; set; }
    public Status Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
