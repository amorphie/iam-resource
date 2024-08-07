using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BBT.Resource.Resources;

public class ResourceRuleMapInput
{
    [Required]
    [MaxLength(ResourceConsts.MaxUrlLength)]
    public string Url { get; set; }
    [Required]
    public string Method { get; set; }
    [Required]
    public string RuleName { get; set; }
    
    public ResourceType[] GetResourceTypes()
    {
        if (string.IsNullOrEmpty(Method))
        {
            return [];
        }

        var methods = Method.Split(',');
        var resourceTypes = new List<ResourceType>();

        foreach (var method in methods)
        {
            if (Enum.TryParse(typeof(ResourceType), method.Trim(), true, out var resourceType))
            {
                resourceTypes.Add((ResourceType)resourceType);
            }
            else
            {
                throw new ArgumentException($"Invalid method: {method}");
            }
        }

        return resourceTypes.ToArray();
    }
}
