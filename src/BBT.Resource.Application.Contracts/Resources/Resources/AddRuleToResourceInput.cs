using System;
using System.ComponentModel.DataAnnotations;

namespace BBT.Resource.Resources;

public class AddRuleToResourceInput
{
    [Required] public Guid RuleId { get; set; }
    public Guid? ClientId { get; set; }

    [MinLength(0)] [MaxLength(10)] public int Priority { get; set; } = 1;
}
