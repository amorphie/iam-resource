using System.ComponentModel.DataAnnotations;

namespace BBT.Resource.Resources;

public class UpdateResourcePolicyInput
{
    [Required]
    [MaxLength(SharedConsts.MaxStatusLength)]
    public string Status { get; set; }
    [Required] public string[] Clients { get; set; }
    [MinLength(0)] [MaxLength(10)] public int Priority { get; set; } = 1;
}
