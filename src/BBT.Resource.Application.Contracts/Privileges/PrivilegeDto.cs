using System;
using BBT.Prism.Application.Dtos;
using BBT.Prism.Auditing;

namespace BBT.Resource.Privileges;

public class PrivilegeDto : EntityDto<Guid>, IHasCreatedAt, IHasModifyTime
{
    public string Url { get; set; }
    public PrivilegeType Type { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
