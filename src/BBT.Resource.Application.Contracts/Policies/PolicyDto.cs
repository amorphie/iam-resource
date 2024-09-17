using System;
using BBT.Prism.Application.Dtos;
using BBT.Prism.Auditing;

namespace BBT.Resource.Policies;

public class PolicyListDto : EntityDto<Guid>, IHasCreatedAt, IHasModifyTime
{
    public string Name { get; set; }
    public Guid? ParentId { get; set; }
    public Effect Effect { get; set; }
    public string[]? Permissions { get; set; }
    public string[]? EvaluationOrder { get; set; }
    public int Priority { get; set; }
    public ConflictResolution ConflictResolution { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public bool Template { get; set; }
}

public class PolicyDto : PolicyListDto
{
    public PolicyConditionDto Condition { get; set; }
}
