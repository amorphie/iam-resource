using System;
using System.Collections.Generic;
using System.Linq;
using BBT.Prism;
using BBT.Prism.Domain.Entities.Auditing;

namespace BBT.Resource.Policies;

public class Policy : AuditedAggregateRoot<Guid>
{
    public string Name { get; private set; }
    public Guid? ParentId { get; set; }
    public Effect Effect { get; private set; }
    public string[]? Permissions { get; private set; }
    public string? PermissionProvider { get; private set; }
    public string[]? EvaluationOrder { get; private set; }
    public int Priority { get; private set; }
    public ConflictResolution ConflictResolution { get; private set; }
    public PolicyCondition Condition { get; private set; }
    public bool Template { get; private set; }
    
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
        Template = false;
    }

    public void SetTemplate(bool template)
    {
        Template = template;
    }

    public void SetName(string name)
    {
        Name = Check.NotNullOrEmpty(name, nameof(Name), PolicyConsts.MaxNameLength);
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

    public void SetPermissions(string? permissionProvider, string[]? permissions)
    {
        PermissionProvider = Check.Length(permissionProvider, nameof(PermissionProvider), PolicyConsts.MaxProviderNameLength);
        Permissions = permissions;
    }

    public void SetEvaluationOrder(string[]? evaluationOrder)
    {
        if (evaluationOrder.IsNullOrEmpty())
        {
            SetDefaultEvaluationOrder();
            return;
        }

#pragma warning disable CS8604 // Possible null reference argument.
        var invalidOrders = evaluationOrder.Except(PolicyConsts.DefaultEvaluationOrder).ToArray();
#pragma warning restore CS8604 // Possible null reference argument.
        if (invalidOrders.Any())
        {
            throw new ArgumentException($"Invalid evaluation order types: {string.Join(", ", invalidOrders)}");
        }

        EvaluationOrder = evaluationOrder;
    }

    private void SetDefaultEvaluationOrder()
    {
        EvaluationOrder = PolicyConsts.DefaultEvaluationOrder;
    }

    public void UpdateCondition(
        ObjectDictionary? context,
        ObjectDictionary? attributes,
        string[]? roles = null,
        string[]? rules = null,
        PolicyTime? time = null
    )
    {
        Condition = new PolicyCondition(context, attributes, roles, rules, time);
    }
}
