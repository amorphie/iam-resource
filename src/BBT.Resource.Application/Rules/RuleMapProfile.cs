using AutoMapper;

namespace BBT.Resource.Rules;

internal class RuleMapProfile : Profile
{
    public RuleMapProfile()
    {
        CreateMap<Rule, RuleDto>();
    }
}
