using System;
using BBT.Prism.Application.Dtos;
using BBT.Prism.Auditing;

namespace BBT.Resource.Rules;

public class RuleDto : EntityDto<Guid>, IHasCreatedAt, IHasModifyTime
{
    public string Name { get; set; }
    public string Expression { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
