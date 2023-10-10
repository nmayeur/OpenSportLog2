using AutoMapper;
using WebAPI.Common.Dto;
using WebAPI.Common.Model;

namespace WebAPI.Common.Utils
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Activity, ActivityDto>();
            CreateMap<Activity, ActivityForListDto>();
            CreateMap<Athlete, AthleteDto>();
        }
    }
}
