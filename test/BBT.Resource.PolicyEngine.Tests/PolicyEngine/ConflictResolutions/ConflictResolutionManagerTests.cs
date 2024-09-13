using Xunit;

namespace BBT.Resource.PolicyEngine.ConflictResolutions;

public class ConflictResolutionManagerTests : PolicyEngineTestBase<PolicyEngineTestModule>
{
    private readonly ConflictResolutionManager _conflictResolutionManager;

    public ConflictResolutionManagerTests()
    {
        _conflictResolutionManager = GetRequiredService<ConflictResolutionManager>();
    }

    [Fact]
    public void Resolve_DenyOverrides_ChoosesDenyOverAllow()
    {
        // Arrange
        var evaluationResults = new List<EvaluationResult>
        {
            new EvaluationResult { IsAllowed = true, PassedConditions = new List<string> { "allow condition" } },
            new EvaluationResult { IsAllowed = false, FailedConditions = new List<string> { "deny condition" } }
        };

        // Act
        var result = _conflictResolutionManager.Resolve("deny-overrides", evaluationResults);

        // Assert
        Assert.False(result.IsAllowed);
        Assert.Equal("deny condition", result.FailedConditions.First());
    }

    [Fact]
    public void Resolve_AllowOverrides_ChoosesAllowOverDeny()
    {
        // Arrange
        var evaluationResults = new List<EvaluationResult>
        {
            new EvaluationResult { IsAllowed = false, FailedConditions = new List<string> { "deny condition" } },
            new EvaluationResult { IsAllowed = true, PassedConditions = new List<string> { "allow condition" } }
        };

        // Act
        var result = _conflictResolutionManager.Resolve("allow-overrides", evaluationResults);

        // Assert
        Assert.True(result.IsAllowed);
        Assert.Equal("allow condition", result.PassedConditions.First());
    }

    [Fact]
    public void Resolve_HighestPriority_ChoosesHighestPriorityPolicy()
    {
        // Arrange
        var evaluationResults = new List<EvaluationResult>
        {
            new EvaluationResult { IsAllowed = false, FailedConditions = new List<string> { "deny condition" } },
            new EvaluationResult { IsAllowed = true, PassedConditions = new List<string> { "allow condition" } }
        };
        
        // Act
        var result = _conflictResolutionManager.Resolve("highest-priority", evaluationResults);

        // Assert
        Assert.True(result.IsAllowed);
        Assert.Equal("allow condition", result.PassedConditions.First());
    }

    [Fact]
    public void Resolve_FirstApplicable_ChoosesFirstApplicablePolicy()
    {
        // Arrange
        var evaluationResults = new List<EvaluationResult>
        {
            new EvaluationResult { IsAllowed = false, FailedConditions = new List<string> { "deny condition" } },
            new EvaluationResult { IsAllowed = true, PassedConditions = new List<string> { "allow condition" } },
            new EvaluationResult { IsAllowed = true, PassedConditions = new List<string> { "allow condition 2" } }
        };

        // Act
        var result = _conflictResolutionManager.Resolve("first-applicable", evaluationResults);

        // Assert
        Assert.True(result.IsAllowed);
        Assert.Equal("allow condition", result.PassedConditions.First());
    }
}
