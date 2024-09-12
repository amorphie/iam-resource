using System.ComponentModel.DataAnnotations;
using System.Linq;
using Shouldly;
using Xunit;

namespace BBT.Resource.Policies;

public class UpdatePolicyInputTests
{
    // 1. Test: If Condition and Permissions are both null then throw an error
    [Fact]
    public void Validate_ShouldReturnError_WhenConditionAndPermissionsAreBothNull()
    {
        // Arrange
        var input = new UpdatePolicyInput
        {
            Name = "Test Policy",
            Condition = null,
            Permissions = null,
            Priority = 1,
            Effect = "A"
        };

        // Act
        var validationResults = input.Validate(new ValidationContext(input)).ToList();

        // Assert
        validationResults.Any(v => v.ErrorMessage.Contains("Condition or Permissions must be defined.")).ShouldBeTrue();
    }
    
    // 2. Test: Permissions is defined, if Condition is null it should not throw an error
    [Fact]
    public void Validate_ShouldPass_WhenPermissionsIsDefinedAndConditionIsNull()
    {
        // Arrange
        var input = new UpdatePolicyInput
        {
            Name = "Test Policy",
            Condition = null,
            Permissions = new[] { "Permission1" },
            Priority = 1,
            Effect = "A"
        };

        // Act
        var validationResults = input.Validate(new ValidationContext(input)).ToList();

        // Assert
        validationResults.Any().ShouldBeFalse();
    }

    // 3. Test: If condition is defined but properties are empty, throw an error
    [Fact]
    public void Validate_ShouldReturnError_WhenConditionIsDefinedButEmpty()
    {
        // Arrange
        var input = new UpdatePolicyInput
        {
            Name = "Test Policy",
            Condition = new PolicyConditionDto(),
            Permissions = null,
            Priority = 1,
            Effect = "A"
        };

        // Act
        var validationResults = input.Validate(new ValidationContext(input)).ToList();

        // Assert
        validationResults.Any(v => v.ErrorMessage.Contains("At least one property of Condition must be defined.")).ShouldBeTrue();
    }

    // 4. Test: If at least one property in Condition is filled, it should not throw an error.
    [Fact]
    public void Validate_ShouldPass_WhenConditionHasAtLeastOneProperty()
    {
        // Arrange
        var input = new UpdatePolicyInput
        {
            Name = "Test Policy",
            Condition = new PolicyConditionDto
            {
                Roles = new[] { "Admin" }
            },
            Permissions = null,
            Priority = 1,
            Effect = "A"
        };

        // Act
        var validationResults = input.Validate(new ValidationContext(input)).ToList();

        // Assert
        validationResults.Any().ShouldBeFalse();
    }

    // 5. Test: If both Condition and Permissions are defined, it should not throw an error.
    [Fact]
    public void Validate_ShouldPass_WhenBothConditionAndPermissionsAreDefined()
    {
        // Arrange
        var input = new UpdatePolicyInput
        {
            Name = "Test Policy",
            Condition = new PolicyConditionDto
            {
                Roles = new[] { "Admin" }
            },
            Permissions = new[] { "Permission1" },
            Priority = 1,
            Effect = "A"
        };

        // Act
        var validationResults = input.Validate(new ValidationContext(input)).ToList();

        // Assert
        validationResults.Any().ShouldBeFalse();
    }
}
