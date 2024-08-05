using System;
using BBT.Prism.Domain.Repositories;

namespace BBT.Resource.Rules;

public interface IRuleRepository : IRepository<Rule, Guid>
{
}
