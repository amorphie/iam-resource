namespace BBT.Resource.Policies;

public class PolicyConsts
{
    public const int MaxNameLength = 120;
    public const int MaxTimezoneLength = 10;
    public const int MaxPriorityLimit = 99999;
    public const int MaxEffectLength = 1;
    public const int MaxConflictResolutionLength = 1;
    public const int MaxProviderNameLength = 10;
    public const string DefaultTimezone = "UTC";
    public static readonly string[] DefaultEvaluationOrder = ["roles", "attributes", "context", "rules", "time"];
    public static readonly string[] ValidEffects = ["A", "D"];
    public static readonly string[] ValidConflictResolutions = ["N", "D", "A", "F"];
}
