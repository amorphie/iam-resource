using System;
using BBT.Prism.Domain.Entities;
using BBT.Prism.MultiLingualEntities;

namespace BBT.Resource.Roles;

public class ScopeTranslation: Entity, IEntityTranslation
{
    public Guid ScopeId { get; set; }
    public string Language { get; set; }
    public string Name { get; set; }
    public override object?[] GetKeys()
    {
        return [ScopeId, Language];
    }
}
