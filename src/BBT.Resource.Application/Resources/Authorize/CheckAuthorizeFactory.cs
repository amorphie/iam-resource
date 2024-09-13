using System;
using Microsoft.Extensions.DependencyInjection;

namespace BBT.Resource.Resources.Authorize;

public enum CheckAuthorizeType
{
    Rule,
    Privilege,
    Policy
}

public interface ICheckAuthorizeFactory
{
    ICheckAuthorize Create(CheckAuthorizeType type);
}

public class CheckAuthorizeFactory(IServiceProvider serviceProvider)
    : ICheckAuthorizeFactory
{
    public ICheckAuthorize Create(CheckAuthorizeType type)
    {
        return type switch
        {
            CheckAuthorizeType.Rule => serviceProvider.GetRequiredService<CheckAuthorizeByRule>(),
            CheckAuthorizeType.Privilege => serviceProvider.GetRequiredService<CheckAuthorizeByPrivilege>(),
            CheckAuthorizeType.Policy => serviceProvider.GetRequiredService<CheckAuthorizeByPolicy>(),
            _ => throw new ArgumentException("Invalid CheckAuthorizeType", nameof(type))
        };
    }
}
