using System;
using System.Collections.Generic;
using BBT.Prism.Domain.Entities.Auditing;
using BBT.Prism.MultiLingualEntities;

namespace BBT.Resource.Roles;

public class RoleGroup : AuditedAggregateRoot<Guid>, IMultiLingualEntity<RoleGroupTranslation>
{
    public string[]? Tags { get; set; }
    public Status Status { get; set; }
    public ICollection<RoleGroupTranslation> Translations { get; set; }
    public ICollection<RoleGroupRole> Roles { get; private set; }
    public ICollection<Scope> Scopes { get; private set; }
    
    
}
