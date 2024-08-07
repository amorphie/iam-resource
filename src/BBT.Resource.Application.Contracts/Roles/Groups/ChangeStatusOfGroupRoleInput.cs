using System.ComponentModel.DataAnnotations;

namespace BBT.Resource.Roles;

public class ChangeStatusOfGroupRoleInput
{
    [Required]
    [MaxLength(SharedConsts.MaxStatusLength)]
    public string Status { get; set; }
}
