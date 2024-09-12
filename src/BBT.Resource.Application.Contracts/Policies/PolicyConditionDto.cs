using System;

namespace BBT.Resource.Policies;

public class PolicyConditionDto
{
    public string[]? Roles { get; set; }
    public PolicyTimeDto? Time { get; set; }
    public string[]? Rules { get; set; }
    public ObjectDictionary? Context { get; set; }
    public ObjectDictionary? Attributes { get; set; }
}

public class PolicyTimeDto
{
    public TimeOnly Start { get; set; }
    public TimeOnly End { get; set; }
    public string Timezone { get; set; }
}
