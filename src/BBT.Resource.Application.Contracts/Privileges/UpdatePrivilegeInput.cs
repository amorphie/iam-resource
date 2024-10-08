using System.ComponentModel.DataAnnotations;

namespace BBT.Resource.Privileges;

public class UpdatePrivilegeInput
{
    [Required]
    [MaxLength(PrivilegeConsts.MaxUrlLength)]
    public string Url { get; set; }
    [Required]
    public PrivilegeType Type { get; set; }
}
