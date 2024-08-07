using System.ComponentModel.DataAnnotations;

namespace BBT.Resource.Resources;

public class UpdateResourcePrivilegeInput
{
    [Required]
    [MaxLength(SharedConsts.MaxStatusLength)]
    public string Status { get; set; }
    [Required]
    [MinLength(0)]
    [MaxLength(10)]
    public int Priority { get; set; }
}
