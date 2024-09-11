using AutoMapper;

namespace BBT.Resource.Policies;

internal class PolicyMapProfile : Profile
{
    public PolicyMapProfile()
    {
        CreateMap<Policy, PolicyDto>();
        CreateMap<Policy, PolicyListDto>();
        CreateMap<PolicyCondition, PolicyConditionDto>();
        CreateMap<PolicyTime, PolicyTimeDto>();
    }
}
