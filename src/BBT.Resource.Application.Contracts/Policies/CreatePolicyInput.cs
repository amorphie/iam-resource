using System;
using BBT.Prism.Application.Dtos;

namespace BBT.Resource.Policies;

public class CreatePolicyInput : UpdatePolicyInput, IEntityDto<Guid>
{
    public Guid Id { get; set; }
}
