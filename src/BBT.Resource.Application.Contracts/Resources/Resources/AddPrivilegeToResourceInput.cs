using System;
using System.ComponentModel.DataAnnotations;

namespace BBT.Resource.Resources;

public class AddPrivilegeToResourceInput
{
    [Required]
    public Guid PrivilegeId { get; set; }
    public Guid? ClientId { get; set; }
    [Required]
    [MinLength(0)]
    [MaxLength(10)]
    public int Priority { get; set; }
}
