using System;
using System.Collections.Generic;
using BBT.Prism.Domain.Entities.Auditing;
using BBT.Prism.MultiLingualEntities;

namespace BBT.Resource.Roles;

public class Scope : AuditedEntity<Guid>, IMultiLingualEntity<ScopeTranslation>
{
    public Guid GroupId { get; set; }
    public Guid ClientId { get; set; }
    public string Reference { get; set; }
    public string[]? Tags { get; set; }
    public Status Status { get; set; }
    public ICollection<ScopeTranslation> Translations { get; set; }
}
