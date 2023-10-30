using AutoMapper;
using WebAPI.Common.Model;
using WebAPI.Dto;
using WebAPI.Dto.Dto;

namespace WebAPI.Common.Utils
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Activity, ActivityDto>();
            CreateMap<Activity, ActivityForListDto>();
            CreateMap<Athlete, AthleteDto>();
            CreateMap<TrackPoint, TrackPointDto>();
        }
    }
}
