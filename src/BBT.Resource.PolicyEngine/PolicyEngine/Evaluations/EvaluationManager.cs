using BBT.Resource.PolicyEngine.Evaluations.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BBT.Resource.PolicyEngine.Evaluations;

internal sealed class EvaluationManager(
    IServiceProvider serviceProvider,
    IUserStore userStore,
    IOptions<EvaluationOptions> options)
{
    private readonly EvaluationOptions _options = options.Value;

    public async Task<EvaluationResult> EvaluateAsync(IPolicy policy, UserRequestContext context)
    {
        var evaluationResult = new EvaluationResult();
        
        if (policy.Permissions != null)
        {
            var userPermissions = await userStore.GetUserPermissionsAsync(context.UserId);
            if (userPermissions != null)
            {
                var hasRequiredPermission = userPermissions.Intersect(policy.Permissions).Any();
                if (!hasRequiredPermission)
                {
                    evaluationResult.IsAllowed = false;
                    evaluationResult.FailedConditions.Add("permissions");
                    evaluationResult.FailureReason = "User does not have the required permissions.";
                    return
                        evaluationResult;
                }
            }
        }

        foreach (var stepName in policy.EvaluationOrder)
        {
            if (_options.Steps.TryGetValue(stepName, out var step))
            {
                var stepResult = false;
                await using (var scope = serviceProvider.CreateAsyncScope())
                {
                    if (scope.ServiceProvider.GetService(step.Provider) is IEvaluationStep service)
                    {
                        stepResult = await service.EvaluateAsync(policy, context);
                    }
                }

                if (policy.Effect == "allow")
                {
                    if (stepResult)
                    {
                        evaluationResult.AddPassedCondition(stepName);
                    }
                    else
                    {
                        evaluationResult.AddFailedCondition(stepName,
                            $"Condition {stepName} failed under 'allow' effect.");
                        evaluationResult.IsAllowed = false;
                        return evaluationResult;
                    }
                }
                else if (policy.Effect == "deny")
                {
                    if (stepResult)
                    {
                        evaluationResult.AddFailedCondition(stepName,
                            $"Condition {stepName} failed under 'deny' effect.");
                        evaluationResult.IsAllowed = false;
                        return evaluationResult; 
                    }

                    evaluationResult.AddPassedCondition(stepName);
                }
            }
        }

        evaluationResult.IsAllowed = policy.Effect == "allow";
        return evaluationResult;
    }
}
