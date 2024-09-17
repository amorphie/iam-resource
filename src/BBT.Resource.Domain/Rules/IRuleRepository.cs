using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BBT.Prism.Domain.Repositories;

namespace BBT.Resource.Rules;

public interface IRuleRepository : IRepository<Rule, Guid>
{
    Task<List<Rule>> GetByIdsAsync(Guid[] ids, CancellationToken cancellationToken = default);
}
