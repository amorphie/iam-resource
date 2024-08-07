using System;
using BBT.Prism.Application.Dtos;
using BBT.Prism.Auditing;

namespace BBT.Resource.Roles;

public class RoleMultiLingualDto : EntityDto<Guid>, IHasCreatedAt, IHasModifyTime
{
    public string Name { get; set; }
    public Guid DefinitionId { get; set; }
    public string DefinitionName { get; set; }
    public string[]? Tags { get; set; }
    public Status Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
