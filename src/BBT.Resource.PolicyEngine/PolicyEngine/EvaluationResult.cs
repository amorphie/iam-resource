using System.Text;

namespace BBT.Resource.PolicyEngine;

public class EvaluationResult
{
    public bool IsAllowed { get; set; }
    public List<string> PassedConditions { get; set; } = new List<string>();
    public List<string> FailedConditions { get; set; } = new List<string>();
    public string FailureReason { get; set; } // En son takıldığı yerin nedeni

    public void AddPassedCondition(string condition)
    {
        PassedConditions.Add(condition);
    }

    public void AddFailedCondition(string condition, string reason)
    {
        FailedConditions.Add(condition);
        FailureReason = reason; // En son başarısız olan koşulun nedeni
    }

    public string GetDetailedReport()
    {
        var report = new StringBuilder();
        report.AppendLine($"Is Allowed: {IsAllowed}");
        report.AppendLine("Passed Conditions:");
        foreach (var condition in PassedConditions)
        {
            report.AppendLine($"- {condition}");
        }

        report.AppendLine("Failed Conditions:");
        foreach (var condition in FailedConditions)
        {
            report.AppendLine($"- {condition}");
        }

        if (!string.IsNullOrEmpty(FailureReason))
        {
            report.AppendLine($"Failure Reason: {FailureReason}");
        }

        return report.ToString();
    }
}
