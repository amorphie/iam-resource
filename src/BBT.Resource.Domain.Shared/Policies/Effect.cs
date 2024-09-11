using System;

namespace BBT.Resource.Policies;

public class Effect : IEquatable<Effect>
{
    public static readonly Effect Allow = new Effect("A", "Allow");
    public static readonly Effect Deny = new Effect("D", "Deny");

    public string Code { get; }
    public string Description { get; }

    private Effect() { }

    private Effect(string code, string description)
    {
        Code = code ?? throw new ArgumentNullException(nameof(code));
        Description = description ?? throw new ArgumentNullException(nameof(description));
    }

    public static Effect FromCode(string code)
    {
        return code switch
        {
            "A" => Allow,
            "D" => Deny,
            _ => throw new ArgumentException($"Unknown status code: {code}")
        };
    }

    public override bool Equals(object obj)
    {
        return obj is Effect other && Equals(other);
    }

    public bool Equals(Effect other)
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
