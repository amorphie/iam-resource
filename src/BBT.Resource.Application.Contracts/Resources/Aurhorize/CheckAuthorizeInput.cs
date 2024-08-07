using System.ComponentModel.DataAnnotations;

namespace BBT.Resource.Resources;

public class CheckAuthorizeInput
{
    [Required]
    public string Url { get; set; }
    public string? Data { get; set; }
    [Required]
    public string Method { get; set; }
}
