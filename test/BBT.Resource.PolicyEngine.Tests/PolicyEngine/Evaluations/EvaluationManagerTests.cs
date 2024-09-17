using Xunit;

namespace BBT.Resource.PolicyEngine.Evaluations;

public class EvaluationManagerTests : PolicyEngineTestBase<PolicyEngineTestModule>
{
    private readonly EvaluationManager _evaluationManager;

    public EvaluationManagerTests()
    {
        _evaluationManager = GetRequiredService<EvaluationManager>();
    }

    [Fact]
    public async Task Evaluate_RolesCondition_PassedAsync()
    {
        // Arrange
        var policy = new PolicyDefinition()
        {
            Conditions = new Conditions
            {
                Roles = new List<string> { "Admin" }
            },
            Effect = "allow",
            EvaluationOrder = new List<string> { "roles" }
        };

        var context = new UserRequestContext
        {
            UrlPattern = "/api/test",
            Roles = new List<string> { "Admin" }
        };

        // Act
        var result = await _evaluationManager.EvaluateAsync(policy, context);

        // Assert
        Assert.True(result.IsAllowed);
        Assert.Contains("roles", result.PassedConditions);
    }

    [Fact]
    public async Task Evaluate_AttributesCondition_FailedAsync()
    {
        // Arrange
        var policy = new PolicyDefinition
        {
            Conditions = new Conditions
            {
                Attributes = new Dictionary<string, string> { { "department", "IT" } }
            },
            Effect = "allow",
            EvaluationOrder = new List<string> { "attributes" }
        };

        var context = new UserRequestContext
        {
            UrlPattern = "/api/test",
            Attributes = new Dictionary<string, string> { { "department", "HR" } }
        };

        // Act
        var result = await _evaluationManager.EvaluateAsync(policy, context);

        // Assert
        Assert.False(result.IsAllowed);
        Assert.Contains("attributes", result.FailedConditions);
        Assert.Equal("Condition attributes failed under 'allow' effect.", result.FailureReason);
    }

    [Fact]
    public async Task Evaluate_TimeCondition_PassedAsync()
    {
        // Arrange
        var policy = new PolicyDefinition
        {
            Conditions = new Conditions
            {
                Time = new TimeCondition
                {
                    Start = "09:00",
                    End = "17:00",
                    Timezone = "UTC"
                }
            },
            Effect = "allow",
            EvaluationOrder = new List<string> { "time" }
        };

        var context = new UserRequestContext
        {
            UrlPattern = "/api/test",
            Time = DateTime.UtcNow.Date.AddHours(10) // 10:00 UTC
        };

        // Act
        var result = await _evaluationManager.EvaluateAsync(policy, context);

        // Assert
        Assert.True(result.IsAllowed);
        Assert.Contains("time", result.PassedConditions);
    }

    [Fact]
    public async Task Evaluate_RulesCondition_PassedAsync()
    {
        // Arrange
        var policy = new PolicyDefinition
        {
            Conditions = new Conditions
            {
                Rules =
                [
                    new RuleCondition
                    {
                        Id = "rule1",
                        Expression = "header.department == \"IT\" && header.clearanceLevel == \"5\""
                    }
                ]
            },
            Effect = "allow",
            EvaluationOrder = ["rules"]
        };

        var context = new UserRequestContext
        {
            UrlPattern = "/api/organizations/([^/]+)",
            RequestUrl = "/api/organizations/u938lkdkd",
            AllHeaders = new Dictionary<string, string>
            {
                { "department", "IT" },
                { "clearanceLevel", "5" }
            },
        };

        // Act
        var result = await _evaluationManager.EvaluateAsync(policy, context);

        // Assert
        Assert.True(result.IsAllowed);
        Assert.Contains("rules", result.PassedConditions);
    }
}
