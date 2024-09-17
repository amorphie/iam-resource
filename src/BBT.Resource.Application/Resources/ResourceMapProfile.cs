using AutoMapper;
using BBT.Resource.Policies;
using BBT.Resource.PolicyEngine;

namespace BBT.Resource.Resources;

internal class ResourceMapProfile : Profile
{
    public ResourceMapProfile()
    {
        CreateMap<ResourceGroupTranslation, ResourceGroupTranslationDto>();
        CreateMap<ResourceGroup, ResourceGroupDto>();

        CreateMap<ResourceTranslation, ResourceTranslationDto>();
        CreateMap<Resource, ResourceDto>();

        CreateMap<Policy, PolicyDefinition>()
            .ForMember(
                dst => dst.Id,
                src => src.MapFrom(m => m.Id.ToString())
            )
            .ForMember(
                dst => dst.ParentId,
                src => src.MapFrom(m => m.ParentId.HasValue ? m.ParentId.ToString() : string.Empty)
            )
            .ForMember(
                dst => dst.Effect,
                src => src.MapFrom(m => m.Effect.Code)
            )
            .ForMember(
                dst => dst.ConflictResolution,
                src => src.MapFrom(m => m.ConflictResolution.Code)
            )
            .ForMember(
                dst => dst.Conditions,
                src => src.MapFrom(m => m.Condition)
            );

        CreateMap<PolicyCondition, Conditions>()
            .ForMember(
                dst => dst.Attributes,
                src =>
                    src.MapFrom(m => m.Attributes)
            )
            .ForMember(
                dst => dst.Context,
                src =>
                    src.MapFrom(m => m.Context)
            )
            .ForMember(
                dst => dst.Rules,
                src => src.Ignore()
            );

        CreateMap<PolicyTime, TimeCondition>()
            .ForMember(
                dst => dst.Start,
                src => src.MapFrom(m => m.Start.ToString())
            )
            .ForMember(
                dst => dst.End,
                src => src.MapFrom(m => m.End.ToString())
            );
    }
}
