namespace BBT.Resource.PolicyEngine.Evaluations;

internal sealed class TimeEvaluationStep : IEvaluationStep
{
    public Task<bool> EvaluateAsync(IPolicy policy, UserRequestContext context)
    {
        var start = TimeSpan.Parse(policy.Conditions.Time.Start);
        var end = TimeSpan.Parse(policy.Conditions.Time.End);

        var timeZone = string.IsNullOrEmpty(policy.Conditions.Time.Timezone)
            ? TimeZoneInfo.Utc
            : TimeZoneInfo.FindSystemTimeZoneById(policy.Conditions.Time.Timezone);

        var userTime = TimeZoneInfo.ConvertTimeFromUtc(context.Time, timeZone);
        var currentTime = userTime.TimeOfDay;

        var result = currentTime >= start && currentTime <= end;
        return Task.FromResult(result);
    }
}
