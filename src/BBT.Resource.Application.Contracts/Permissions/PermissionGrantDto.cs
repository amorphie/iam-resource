namespace BBT.Resource.Permissions;

public class PermissionGrantDto
{
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string Description { get; set; }
    public string ParentName { get; set; }
    public bool IsGranted { get; set; }
    public string? Provider { get; set; }
}
