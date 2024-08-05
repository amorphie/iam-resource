using System;
using BBT.Prism;
using BBT.Prism.Auditing;
using BBT.Prism.Domain.Entities;
using BBT.Prism.Domain.Entities.Auditing;

namespace BBT.Resource.Rules;

public class Rule : AuditedEntity<Guid>
{
    public string Name { get; private set; }
    public string Expression { get; private set; }

    private Rule()
    {
        //For Orm
    }

    public Rule(Guid id, string name, string expression) : base(id)
    {
        Name = name;
        Expression = expression;
    }

    public void SetName(string name)
    {
        Name = Check.NotNullOrEmpty(name, nameof(Name), RuleConsts.MaxNameLength);
    }

    public void SetExpression(string expression)
    {
        Name = Check.NotNullOrEmpty(expression, nameof(Expression));
    }
}
