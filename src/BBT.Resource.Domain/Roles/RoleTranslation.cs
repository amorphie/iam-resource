using System;
using BBT.Prism.Domain.Entities;
using BBT.Prism.MultiLingualEntities;

namespace BBT.Resource.Roles;

public class RoleTranslation: Entity, IEntityTranslation
{
    public Guid RoleId { get; set; }
    public string Language { get; set; }
    public string Name { get; set; }
    
    public override object?[] GetKeys()
    {
        return [RoleId, Language];
    }
}
