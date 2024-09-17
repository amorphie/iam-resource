using BBT.Resource.PolicyEngine.Evaluations.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BBT.Resource.PolicyEngine.Evaluations;

internal sealed class EvaluationManager(
    IServiceProvider serviceProvider,
    IPermissionStore permissionStore,
    IOptions<EvaluationOptions> options)
{
    private readonly EvaluationOptions _options = options.Value;

    public async Task<EvaluationResult> EvaluateAsync(IPolicy policy, UserRequestContext context)
    {
        var evaluationResult = new EvaluationResult();

        if (policy.Permissions != null)
        {
            if (policy.Permissions.Any())
            {
                if (IsValidPermissionContext(context, policy))
                {
                    var hasRequiredPermission = await permissionStore.CheckAsync(
                        context.FindApplicationId()!,
                        clientId: context.FindClientId()!,
                        providerName: policy.PermissionProvider!,
                        providerKey: context.FindProviderKeyByProvider(policy.PermissionProvider!)!,
                        policy.Permissions.ToArray()
                    );

                    if (!hasRequiredPermission)
                    {
                        evaluationResult.IsAllowed = false;
                        evaluationResult.FailedConditions.Add("permissions");
                        evaluationResult.FailureReason = "User does not have the required permissions.";
                        return
                            evaluationResult;
                    }
                }
                else
                {
                    evaluationResult.IsAllowed = false;
                    evaluationResult.FailedConditions.Add("permissions");
                    evaluationResult.FailureReason =
                        "Context information for the permission control is missing or incorrect.";
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

    /// <summary>
    /// Validates the permission context by checking if the necessary application, client, and provider information 
    /// is available in the user context and matches the specified policy.
    /// </summary>
    /// <param name="userContext">The context containing user-specific information such as application ID, client ID, and provider key.</param>
    /// <param name="policy">The policy that defines the permission provider for validation.</param>
    /// <returns>
    /// Returns <c>true</c> if the user context contains valid application ID, client ID, or provider key for the specified policy; 
    /// otherwise, returns <c>false</c>.
    /// </returns>
    private bool IsValidPermissionContext(UserRequestContext userContext, IPolicy policy)
    {
        var applicationId = userContext.FindApplicationId();
        var clientId = userContext.FindClientId();
        var provider = policy.PermissionProvider;
        if (applicationId.IsNullOrEmpty() && clientId.IsNullOrEmpty() && provider.IsNullOrEmpty())
        {
            return false;
        }

        if (userContext.FindProviderKeyByProvider(provider!).IsNullOrEmpty())
        {
            return false;
        }

        return true;
    }
}
