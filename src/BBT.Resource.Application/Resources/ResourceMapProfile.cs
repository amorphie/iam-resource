using AutoMapper;

namespace BBT.Resource.Resources;

internal class ResourceMapProfile : Profile
{
    public ResourceMapProfile()
    {
        CreateMap<ResourceGroupTranslation, ResourceGroupTranslationDto>();
        CreateMap<ResourceGroup, ResourceGroupDto>();
        
        CreateMap<ResourceTranslation, ResourceTranslationDto>();
        CreateMap<Resource, ResourceDto>();
    }
}
