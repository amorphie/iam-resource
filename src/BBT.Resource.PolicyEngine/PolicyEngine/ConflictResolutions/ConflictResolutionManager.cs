using BBT.Resource.PolicyEngine.ConflictResolutions.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BBT.Resource.PolicyEngine.ConflictResolutions;

/// <summary>
/// Defines the strategy for resolving conflicts between multiple applicable policies.
/// </summary>
/// <remarks>
/// When multiple policies apply to a user or resource,
/// Conflict Resolution strategies ensure that the system consistently handles situations
/// where one policy might allow access, and another might deny it.
/// </remarks>
/// <param name="serviceProvider">Service Provider</param>
/// <param name="options">Defined strategy options</param>
internal sealed class ConflictResolutionManager(
    IServiceProvider serviceProvider,
    IOptions<ConflictResolutionOptions> options)
{
    private readonly ConflictResolutionOptions _options = options.Value;

    public EvaluationResult Resolve(string conflictResolution, List<EvaluationResult> evaluationResults)
    {
        using var scope = serviceProvider.CreateScope();
        var strategies = _options.Strategies;

        if (strategies.TryGetValue(conflictResolution, out var strategy))
        {
            if (scope.ServiceProvider.GetService(strategy.Provider) is IConflictResolutionStrategy strategyService)
            {
                return strategyService.Resolve(evaluationResults);
            }
        }

        if (scope.ServiceProvider.GetService(GetDefaultStrategy(strategies)) is
            IConflictResolutionStrategy
            denyOverrideService)
        {
            return denyOverrideService.Resolve(evaluationResults);
        }

        throw new InvalidOperationException("Conflict resolution strategy could not be resolved.");
    }

    private Type GetDefaultStrategy(ConflictResolutionConfigurationDictionary strategies)
    {
        return strategies
            .First(s => s.Value.Default)
            .Value
            .Provider;
    }
}
