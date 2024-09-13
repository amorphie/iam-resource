using Xunit;

namespace BBT.Resource.PolicyEngine;

public class PolicyEngineTests: PolicyEngineTestBase<PolicyEngineTestModule>
{
    private readonly IPolicyEngine _policyEngine;

    public PolicyEngineTests()
    {
        _policyEngine = GetRequiredService<IPolicyEngine>();
    }
    [Fact]
    public async Task EvaluatePolicies_MultiplePoliciesWithDenyOverridesAsync()
    {
        // Arrange
        var policies = new List<IPolicy>
        {
            new PolicyDefinition
            {
                Id = "policy1",
                Name = "policy1",
                Conditions = new Conditions
                {
                    Roles = ["Admin"]
                },
                Effect = "allow",
                EvaluationOrder = ["roles"],
                Priority = 2,
                ConflictResolution = "deny-overrides"
            },
            new PolicyDefinition
            {
                Id = "policy2",
                Name = "policy2",
                Conditions = new Conditions
                {
                    Roles = ["User"]
                },
                Effect = "deny",
                EvaluationOrder = ["roles"],
                Priority = 1,
                ConflictResolution = "deny-overrides"
            }
        };

        var context = new UserRequestContext
        {
            Roles = new List<string> { "Admin" }
        };
        
        // Act
        var result = await _policyEngine.EvaluatePoliciesAsync(policies, context);

        // Assert
        Assert.False(result.IsAllowed);
    }

    [Fact]
    public async Task EvaluatePolicies_DenyOverrides_PolicyFailsDueToDenyAsync()
    {
        // Arrange
        var policies = new List<IPolicy>
        {
            new PolicyDefinition
            {
                Id = "policy1",
                Name = "policy1",
                Conditions = new Conditions
                {
                    Roles = ["Admin"]
                },
                Effect = "allow",
                EvaluationOrder = ["roles"],
                Priority = 2,
                ConflictResolution = "deny-overrides"
            },
            new PolicyDefinition
            {
                Id = "policy2",
                Name = "policy2",
                Conditions = new Conditions
                {
                    Roles = ["User"]
                },
                Effect = "deny",
                EvaluationOrder = ["roles"],
                Priority = 1,
                ConflictResolution = "deny-overrides"
            }
        };

        var context = new UserRequestContext
        {
            Roles = new List<string> { "User" }
        };
        
        // Act
        var result = await _policyEngine.EvaluatePoliciesAsync(policies, context);

        // Assert
        Assert.False(result.IsAllowed);
    }

    [Fact]
    public async Task EvaluatePolicies_AllowOverrides_PolicyPassesIfAnyAllowAsync()
    {
        // Arrange
        var policies = new List<IPolicy>
        {
            new PolicyDefinition
            {
                Id = "policy1",
                Name = "policy1",
                Conditions = new Conditions
                {
                    Roles = ["Admin"]
                },
                Effect = "allow",
                EvaluationOrder = ["roles"],
                Priority = 2,
                ConflictResolution = "allow-overrides"
            },
            new PolicyDefinition
            {
                Id = "policy2",
                Name = "policy2",
                Conditions = new Conditions
                {
                    Roles = ["User"]
                },
                Effect = "deny",
                EvaluationOrder = ["roles"],
                Priority = 1,
                ConflictResolution = "allow-overrides"
            }
        };

        var context = new UserRequestContext
        {
            Roles = new List<string> { "Admin" }
        };
        
        // Act
        var result = await _policyEngine.EvaluatePoliciesAsync(policies, context);

        // Assert
        Assert.True(result.IsAllowed);
    }
}
