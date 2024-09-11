using System;
using System.Collections.Generic;
using BBT.Prism;
using BBT.Prism.Domain.Entities.Auditing;

namespace BBT.Resource.Policies;

public class Policy : AuditedAggregateRoot<Guid>
{
    public string Name { get; private set; }
    public Guid? ParentId { get; set; }
    public Effect Effect { get; private set; }
    public string[]? Permissions { get; private set; }
    public string[]? EvaluationOrder { get; private set; }
    public int Priority { get; private set; }
    public ConflictResolution ConflictResolution { get; private set; }
    public PolicyCondition Condition { get; private set; }


    private Policy()
    {
        //For ORM
    }

    public Policy(
        Guid id,
        string name,
        Effect effect,
        int priority = 999)
        : base(id)
    {
        SetName(name);
        Name = name;
        Effect = effect;
        SetPriority(priority);
        ConflictResolution = ConflictResolution.NA;
        SetDefaultEvaluationOrder();
    }

    public void SetName(string name)
    {
        Name = Check.NotNullOrEmpty(name, nameof(name), PolicyConsts.MaxNameLength);
    }

    public void SetPriority(int priority)
    {
        if (priority < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(Priority),
                $"The value must be in the range of 1 to {PolicyConsts.MaxPriorityLimit}.");
        }

        Priority = priority;
    }

    public void SetPermissions(string[]? permissions)
    {
        Permissions = permissions;
    }

    public void SetEvaluationOrder(string[]? evaluationOrder)
    {
        if (evaluationOrder.IsNullOrEmpty())
        {
            SetDefaultEvaluationOrder();
            return;
        }

        //TODO: Evaluation order'da olmayan bir type handle et.
        EvaluationOrder = evaluationOrder;
    }

    private void SetDefaultEvaluationOrder()
    {
        EvaluationOrder = PolicyConsts.DefaultEvaluationOrder;
    }

    public void UpdateCondition(
        ObjectDictionary context,
        ObjectDictionary attributes,
        string[]? roles = null,
        string[]? rules = null,
        PolicyTime? time = null
    )
    {
        Condition = new PolicyCondition(context, attributes, roles, rules, time);
    }
}
