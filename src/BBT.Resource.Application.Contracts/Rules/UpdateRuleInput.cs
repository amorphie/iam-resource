using System.ComponentModel.DataAnnotations;

namespace BBT.Resource.Rules;

public class UpdateRuleInput
{
    [Required]
    [MaxLength(RuleConsts.MaxNameLength)]
    public string Name { get; set; }

    [Required] public string Expression { get; set; }
}
