using System;
using System.Collections.Generic;
using BBT.Prism.Auditing;
using BBT.Prism.Domain.Entities;
using BBT.Prism.Domain.Entities.Auditing;
using BBT.Prism.MultiLingualEntities;

namespace BBT.Resource.Roles;

public class Role: AuditedEntity<Guid>, IMultiLingualEntity<RoleTranslation>
{
    public Guid DefinitionId { get; private set; }
    public string[]? Tags { get; set; }
    public Status Status { get; private set; }
    public ICollection<RoleTranslation> Translations { get; set; }
}
