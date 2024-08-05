using System;

namespace BBT.Resource;

public class Status: IEquatable<Status>
{
    public static readonly Status Active = new Status("A", "Active");
    public static readonly Status Passive = new Status("P", "Passive");

    public string Code { get; }
    public string Description { get; }

    private Status(string code, string description)
    {
        Code = code;
        Description = description;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as Status);
    }

    public bool Equals(Status other)
    {
        return other != null &&
               Code == other.Code;
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
