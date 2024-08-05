using System;
using System.ComponentModel.DataAnnotations;
using BBT.Prism.Application.Dtos;

namespace BBT.Resource.Privileges;

public class CreatePrivilegeInput : EntityDto<Guid>
{
    [Required]
    [MaxLength(PrivilegeConsts.MaxUrlLength)]
    public string Url { get; set; }

    public PrivilegeType Type { get; set; }
}
