namespace BBT.Resource.Permissions;

public class CheckPermissionResultDto(bool isGranted)
{
    public bool IsGranted { get; set; } = isGranted;
}
