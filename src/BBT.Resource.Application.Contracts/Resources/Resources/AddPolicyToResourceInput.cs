using System;
using System.ComponentModel.DataAnnotations;

namespace BBT.Resource.Resources;

public class AddPolicyToResourceInput
{
    [Required] public Guid PolicyId { get; set; }
    [Required] public string[] Clients { get; set; }
    [MinLength(0)] [MaxLength(10)] public int Priority { get; set; } = 1;
}
