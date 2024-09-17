using System;

namespace BBT.Resource.Policies;

public class ConflictResolution : IEquatable<ConflictResolution>
{
    public static readonly ConflictResolution NA = new ConflictResolution("N", "n/a");
    public static readonly ConflictResolution DenyOverrides = new ConflictResolution("D", "deny-overrides");
    public static readonly ConflictResolution AllowOverrides = new ConflictResolution("A", "allow-overrides");
    public static readonly ConflictResolution FirstApplicable = new ConflictResolution("F", "first-applicable");
    public static readonly ConflictResolution HighestPriority = new ConflictResolution("H", "highest-priority");

    public string Code { get; }
    public string Description { get; }

    private ConflictResolution() { }

    private ConflictResolution(string code, string description)
    {
        Code = code ?? throw new ArgumentNullException(nameof(code));
        Description = description ?? throw new ArgumentNullException(nameof(description));
    }

    public static ConflictResolution FromCode(string code)
    {
        return code switch
        {
            "N" => NA,
            "D" => DenyOverrides,
            "A" => AllowOverrides,
            "F" => FirstApplicable,
            "H" => HighestPriority,
            _ => NA
        };
    }

    public override bool Equals(object obj)
    {
        return obj is ConflictResolution other && Equals(other);
    }

    public bool Equals(ConflictResolution other)
    {
        return Code == other.Code;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Code);
    }

    public override string ToString()
    {
        return $"{Description} ({Code})";
    }
}
