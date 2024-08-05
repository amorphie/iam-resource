using System;
using BBT.Prism.Domain.Entities;
using BBT.Prism.MultiLingualEntities;

namespace BBT.Resource.Resources;

public class ResourceTranslation : Entity, IEntityTranslation
{
    public Guid ResourceId { get; set; }
    public string Language { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    public override object?[] GetKeys()
    {
        return [ResourceId, Language];
    }
}
