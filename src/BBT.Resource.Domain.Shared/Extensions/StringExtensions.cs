using System.Collections.Generic;
using System.Text;
using BBT.Resource.Resources;

namespace BBT.Resource.Extensions;

public static class StringExtensions
{
    public static string? ToAscii(this string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }

        // Create a StringBuilder to store the ASCII converted value
        StringBuilder asciiStringBuilder = new StringBuilder();

        // Define a dictionary for Turkish character replacements
        var turkishCharMap = new Dictionary<char, char>
        {
            { 'ç', 'c' },
            { 'ğ', 'g' },
            { 'ı', 'i' },
            { 'ö', 'o' },
            { 'ş', 's' },
            { 'ü', 'u' },
            { 'Ç', 'C' },
            { 'Ğ', 'G' },
            { 'İ', 'I' },
            { 'Ö', 'O' },
            { 'Ş', 'S' },
            { 'Ü', 'U' }
        };

        // Loop through each character in the input string
        foreach (char c in value)
        {
            // Check if the character is a Turkish character and replace it
            if (turkishCharMap.ContainsKey(c))
            {
                asciiStringBuilder.Append(turkishCharMap[c]);
            }
            else if (c <= sbyte.MaxValue)
            {
                // If it is an ASCII character, append it as is
                asciiStringBuilder.Append(c);
            }
            else
            {
                // If it is not, replace it with a placeholder character or ignore it
                // Here we use '?' as a placeholder, but you can choose any character or omit this line
                asciiStringBuilder.Append('?');
            }
        }

        // Return the ASCII converted string
        return asciiStringBuilder.ToString();
    }

    public static string ToClean(this string value)
    {
        return value
            .Replace("-", "_")
            .Replace(".", "_")
            .Replace(";", "_")
            .Replace(",", "_")
            .Replace(":", "_")
            .Replace("*", "_");
    }

    public static ResourceType ToResourceType(this string value)
    {
        return value.ToLower() switch
        {
            "connect" => ResourceType.CONNECT,
            "delete" => ResourceType.DELETE,
            "get" => ResourceType.GET,
            "head" => ResourceType.HEAD,
            "options" => ResourceType.OPTIONS,
            "post" => ResourceType.POST,
            "put" => ResourceType.PUT,
            "trace" => ResourceType.TRACE,
            _ => ResourceType.ALL
        };
    }
}
