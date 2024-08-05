using System;
using System.ComponentModel.DataAnnotations;
using BBT.Prism.Application.Dtos;

namespace BBT.Resource.Rules;

public class CreateRuleInput : EntityDto<Guid>
{
    [Required]
    [MaxLength(RuleConsts.MaxNameLength)]
    public string Name { get; set; }

    [Required] public string Expression { get; set; }
}
