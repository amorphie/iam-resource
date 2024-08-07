namespace BBT.Resource.Resources;

public class ResourceMapResultModel(Resource resource, bool exists)
{
    public Resource Resource { get; } = resource;
    public bool Exists { get; } = exists;
}
