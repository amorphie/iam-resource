namespace BBT.Resource.Permissions;

public class PermissionGrantInfo
{
    public string Name { get; set; }
    public string ParentName { get; set; }
    public bool IsGranted { get; set; }
    public string Provider { get; set; }
}
