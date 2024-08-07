using System.ComponentModel.DataAnnotations;

namespace BBT.Resource.Resources;

public class ChangeStatusOfResourcePrivilegeInput
{
    [Required]
    [MaxLength(SharedConsts.MaxStatusLength)]
    public string Status { get; set; }
}
