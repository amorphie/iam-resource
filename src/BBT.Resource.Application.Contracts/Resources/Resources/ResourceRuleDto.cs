using System;
using BBT.Prism.Auditing;

namespace BBT.Resource.Resources;

public class ResourceRuleDto: IHasCreatedAt, IHasModifyTime
{
    public Guid RuleId { get; set; }
    public Guid? ClientId { get; set; }
    public Status Status { get; set; }
    public int Priority { get; set; }
    public string RuleName { get; set; }
    public string RuleExpression { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
