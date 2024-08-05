using System;
using BBT.Prism;
using BBT.Prism.Domain.Entities.Auditing;

namespace BBT.Resource.Privileges;

public class Privilege : AuditedEntity<Guid>
{
    public string Url { get; private set; }
    public PrivilegeType Type { get; set; }

    private Privilege()
    {
        //For Orm
    }

    public Privilege(
        Guid id,
        string url,
        PrivilegeType type)
        : base(id)
    {
        Url = url;
        Type = type;
    }

    public void SetUrl(string url)
    {
        Url = Check.NotNullOrEmpty(url, nameof(Url), PrivilegeConsts.MaxUrlLength);
    }
}
