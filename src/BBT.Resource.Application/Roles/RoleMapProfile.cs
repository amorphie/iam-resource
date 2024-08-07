using AutoMapper;

namespace BBT.Resource.Roles;

internal class RoleMapProfile : Profile
{
    public RoleMapProfile()
    {
        CreateMap<RoleDefinitionTranslation, RoleDefinitionTranslationDto>();
        CreateMap<RoleDefinition, RoleDefinitionDto>();

        CreateMap<RoleGroupTranslation, RoleGroupTranslationDto>();
        CreateMap<RoleGroup, RoleGroupDto>();
        
        CreateMap<RoleTranslation, RoleTranslationDto>();
        CreateMap<Role, RoleDto>();
    }
}
