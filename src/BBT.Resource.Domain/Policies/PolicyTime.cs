using System;
using System.Collections.Generic;
using BBT.Prism;
using BBT.Prism.Domain.Values;

namespace BBT.Resource.Policies;

public class PolicyTime : ValueObject
{
    private PolicyTime()
    {
        //For ORM
    }
    
    public PolicyTime(TimeOnly start, TimeOnly end, string timezone)
    {
        if (timezone.IsNullOrEmpty())
        {
            timezone = PolicyConsts.DefaultTimezone;
        }

        Start = start;
        End = end;
        Timezone = Check.NotNullOrEmpty(timezone, nameof(timezone), PolicyConsts.MaxTimezoneLength);
    }

    public TimeOnly Start { get; private set; }
    public TimeOnly End { get; private set; }
    public string Timezone { get; private set; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Start;
        yield return End;
        yield return Timezone;
    }
}
