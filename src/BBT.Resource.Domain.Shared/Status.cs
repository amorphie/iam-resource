using System;

namespace BBT.Resource;

public class Status : IEquatable<Status>
{
    public static readonly Status Active = new Status("A", "Active");
    public static readonly Status Passive = new Status("P", "Passive");

    public string Code { get; }
    public string Description { get; }

    private Status() { }

    private Status(string code, string description)
    {
        Code = code ?? throw new ArgumentNullException(nameof(code));
        Description = description ?? throw new ArgumentNullException(nameof(description));
    }

    public static Status FromCode(string code)
    {
        return code switch
        {
            "A" => Active,
            "P" => Passive,
            _ => throw new ArgumentException($"Unknown status code: {code}")
        };
    }

    public override bool Equals(object obj)
    {
        return obj is Status other && Equals(other);
    }

    public bool Equals(Status other)
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
