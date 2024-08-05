using AutoMapper;

namespace BBT.Resource.Privileges;

internal class PrivilegeMapProfile : Profile
{
    public PrivilegeMapProfile()
    {
        CreateMap<Privilege, PrivilegeDto>();
    }
}
