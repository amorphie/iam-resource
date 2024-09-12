using System;
using System.Collections.Generic;
using BBT.Prism.Domain.Values;

namespace BBT.Resource.Policies;

public class PolicyCondition : ValueObject
{
    private PolicyCondition()
    {
        //For ORM
    }

    public PolicyCondition(
        ObjectDictionary? context,
        ObjectDictionary? attributes,
        string[]? roles = null,
        string[]? rules = null,
        PolicyTime? time = null
    )
    {
        Context = context ?? new ObjectDictionary();
        Attributes = attributes ?? new ObjectDictionary();
        Roles = roles ?? [];
        Rules = rules ?? [];
        Time = time;
    }

    public string[] Roles { get; private set; }
    public PolicyTime? Time { get; private set; }
    public string[] Rules { get; private set; }
    public ObjectDictionary Context { get; private set; }
    public ObjectDictionary Attributes { get; private set; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Roles;
        yield return Time ?? new PolicyTime(new TimeOnly(0, 0, 0), new TimeOnly(0, 0, 0));
        yield return Rules;
        yield return Context;
        yield return Attributes;
    }
}
