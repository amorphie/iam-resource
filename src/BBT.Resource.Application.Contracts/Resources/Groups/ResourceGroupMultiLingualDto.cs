using System;
using BBT.Prism.Application.Dtos;
using BBT.Prism.Auditing;

namespace BBT.Resource.Resources;

public class ResourceGroupMultiLingualDto: EntityDto<Guid>, IHasCreatedAt, IHasModifyTime
{
    public string Name { get; set; }
    public string[]? Tags { get; set; }
    public Status Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
