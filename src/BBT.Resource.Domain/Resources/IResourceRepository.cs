using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BBT.Prism.Domain.Repositories;

namespace BBT.Resource.Resources;

public interface IResourceRepository : IRepository<Resource, Guid>
{
    Task<Resource> GetWithRuleAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Resource> GetWithPrivilegeAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<ResourceRuleModel>> GetRulesAsync(Guid resourceId, CancellationToken cancellationToken = default);
    Task<ResourceRuleModel> GetRuleAsync(Guid resourceId, Guid ruleId, CancellationToken cancellationToken = default);
    Task<List<ResourcePrivilegeModel>> GetPrivilegesAsync(Guid resourceId, CancellationToken cancellationToken = default);
    Task<ResourcePrivilegeModel> GetPrivilegeAsync(Guid resourceId, Guid privilegeId,
        CancellationToken cancellationToken = default);
}
