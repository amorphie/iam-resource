using BBT.Prism.Modularity;
using BBT.Resource.PolicyEngine.ConflictResolutions;
using BBT.Resource.PolicyEngine.ConflictResolutions.Options;
using BBT.Resource.PolicyEngine.Evaluations;
using BBT.Resource.PolicyEngine.Evaluations.Options;
using BBT.Resource.PolicyEngine.Rules;
using Microsoft.Extensions.DependencyInjection;

namespace BBT.Resource.PolicyEngine;

public class PolicyEngineModule : PrismModule
{
    public override void ConfigureServices(ModuleConfigurationContext context)
    {
        Configure<EvaluationOptions>(options =>
        {
            options.Steps.Add(
                new EvaluationStepConfiguration(
                    "roles",
                    "Roles",
                    typeof(RolesEvaluationStep))
            );

            options.Steps.Add(
                new EvaluationStepConfiguration(
                    "attributes",
                    "Attributes",
                    typeof(AttributesEvaluationStep))
            );

            options.Steps.Add(
                new EvaluationStepConfiguration(
                    "rules",
                    "Rules",
                    typeof(RulesEvaluationStep))
            );

            options.Steps.Add(
                new EvaluationStepConfiguration(
                    "time",
                    "Time",
                    typeof(TimeEvaluationStep))
            );

            options.Steps.Add(
                new EvaluationStepConfiguration(
                    "context",
                    "Context",
                    typeof(ContextEvaluationStep))
            );
        });

        Configure<ConflictResolutionOptions>(options =>
        {
            options.Strategies.Add(
                new ConflictResolutionStrategyConfiguration(
                    "deny-overrides",
                    "Deny Overrides",
                    typeof(DenyOverridesStrategy),
                    description: "Deny policies take precedence over allow policies.",
                    isDefault: true)
            );

            options.Strategies.Add(
                new ConflictResolutionStrategyConfiguration(
                    "allow-overrides",
                    "Allow Overrides",
                    typeof(AllowOverridesStrategy),
                    description: "Allow policies take precedence over deny policies.")
            );

            options.Strategies.Add(
                new ConflictResolutionStrategyConfiguration(
                    "highest-priority",
                    "Highest Priority",
                    typeof(HighestPriorityStrategy),
                    description: "The policy with the highest priority (lowest numerical value) is enforced.")
            );

            options.Strategies.Add(
                new ConflictResolutionStrategyConfiguration(
                    "first-applicable",
                    "First Applicable",
                    typeof(FirstApplicableStrategy),
                    description: "The first applicable policy in evaluation order is enforced.")
            );
        });


        context.Services.AddTransient<IRuleEngine, RuleEngine>();
        context.Services.AddTransient<IPolicyEngine, PolicyEngine>();

        context.Services.AddTransient<EvaluationManager>();
        context.Services.AddTransient<AttributesEvaluationStep>();
        context.Services.AddTransient<ContextEvaluationStep>();
        context.Services.AddTransient<RolesEvaluationStep>();
        context.Services.AddTransient<RulesEvaluationStep>();
        context.Services.AddTransient<TimeEvaluationStep>();

        context.Services.AddTransient<ConflictResolutionManager>();
        context.Services.AddTransient<AllowOverridesStrategy>();
        context.Services.AddTransient<DenyOverridesStrategy>();
        context.Services.AddTransient<FirstApplicableStrategy>();
        context.Services.AddTransient<HighestPriorityStrategy>();
    }
}
