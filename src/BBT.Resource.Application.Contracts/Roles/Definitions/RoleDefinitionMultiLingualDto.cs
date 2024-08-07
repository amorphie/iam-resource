using System;
using System.Collections.Generic;
using BBT.Prism.Application.Dtos;
using BBT.Prism.Auditing;

namespace BBT.Resource.Roles;

public class RoleDefinitionMultiLingualDto : EntityDto<Guid>, IHasCreatedAt, IHasModifyTime
{
    public string Key { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public Guid ClientId { get; set; }
    public string[]? Tags { get; set; }
    public Status Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
