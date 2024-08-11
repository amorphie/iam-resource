using System.ComponentModel.DataAnnotations;

namespace BBT.Resource.Resources;

public class UpdateResourceRuleInput
{
    [Required]
    [MaxLength(SharedConsts.MaxStatusLength)]
    public string Status { get; set; }

    [MinLength(0)] [MaxLength(10)] public int Priority { get; set; } = 1;
}
