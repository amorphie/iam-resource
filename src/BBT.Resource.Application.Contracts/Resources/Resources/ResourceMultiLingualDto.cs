using System;
using BBT.Prism.Application.Dtos;
using BBT.Prism.Auditing;

namespace BBT.Resource.Resources;

public class ResourceMultiLingualDto : EntityDto<Guid>, IHasCreatedAt, IHasModifyTime
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public Guid? GroupId { get; set; }
    public ResourceType Type { get; set; }
    public string Url { get; set; }
    public string[]? Tags { get; set; }
    public Status Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
