using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BBT.Resource.Policies;

public class UpdatePolicyInput : IValidatableObject
{
    [Required]
    [MaxLength(PolicyConsts.MaxNameLength)]
    public string Name { get; set; }

    public Guid? ParentId { get; set; }

    [Required]
    [MaxLength(PolicyConsts.MaxEffectLength)]
    public string Effect { get; set; }

    public string[]? Permissions { get; set; }
    public string[]? EvaluationOrder { get; set; }

    [Required]
    [Range(1, PolicyConsts.MaxPriorityLimit)]
    public int Priority { get; set; }

    [MaxLength(PolicyConsts.MaxEffectLength)]
    public string ConflictResolution { get; set; }

    public PolicyConditionDto? Condition { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var validationErrors = new List<ValidationResult>();
        
        // Condition ve Permissions aynı anda null olamaz.
        if (Condition == null && (Permissions == null || Permissions.Length == 0))
        {
            validationErrors.Add(new ValidationResult("Condition or Permissions must be defined.", new[] { nameof(Condition), nameof(Permissions) }));
        }
        
        // Eğer Condition tanımlıysa, en az bir özelliğinin dolu olması gerekir.
        if (Condition != null)
        {
            bool hasAnyConditionProperty = 
                (Condition.Roles != null && Condition.Roles.Length > 0) ||
                Condition.Time != null ||
                (Condition.Rules != null && Condition.Rules.Length > 0) ||
                (Condition.Context != null && Condition.Context.Count > 0) ||
                (Condition.Attributes != null && Condition.Attributes.Count > 0);

            if (!hasAnyConditionProperty)
            {
                validationErrors.Add(new ValidationResult("At least one property of Condition must be defined.", new[] { nameof(Condition) }));
            }
        }

        if (!PolicyConsts.ValidEffects.Contains(Effect))
        {
            validationErrors.Add(new ValidationResult(
                $"Invalid value for Effect. Valid values are: {string.Join(", ", PolicyConsts.ValidEffects)}",
                new[] { nameof(Effect) }));
        }

        if (!string.IsNullOrEmpty(ConflictResolution) &&
            !PolicyConsts.ValidConflictResolutions.Contains(ConflictResolution))
        {
            validationErrors.Add(new ValidationResult(
                $"Invalid value for ConflictResolution. Valid values are: {string.Join(", ", PolicyConsts.ValidConflictResolutions)}",
                new[] { nameof(ConflictResolution) }));
        }

        if (EvaluationOrder != null)
        {
            foreach (var eval in EvaluationOrder)
            {
                if (!PolicyConsts.DefaultEvaluationOrder.Contains(eval))
                {
                    validationErrors.Add(new ValidationResult(
                        $"Invalid value in EvaluationOrder: {eval}. Valid values are: {string.Join(", ", PolicyConsts.DefaultEvaluationOrder)}",
                        new[] { nameof(EvaluationOrder) }));
                }
            }
        }

        return validationErrors;
    }
}
