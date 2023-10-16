using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Dto;
using WebAPI.Common.ViewModel;

namespace WebAPI.Common.Queries
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityQueries _activityQueries;
        private readonly IMapper _mapper;


        public ActivityController(IActivityQueries activityQueries, IMapper mapper)
        {
            _activityQueries = activityQueries ?? throw new ArgumentNullException(nameof(activityQueries));
            _mapper = mapper;
        }

        [HttpGet]
        [Route("activitiesByAthlete")]
        [ProducesResponseType(typeof(PaginatedItemsViewModel<ActivityForListDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PaginatedItemsViewModel<ActivityForListDto>>> GetActivitiesByAthleteAsync(int athleteId, [FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            Console.WriteLine($"Called ActivitiesByAthleteAsync athleteId={athleteId}, pgeSize={pageSize}, pageIndex={pageIndex}");
            var activities = await _activityQueries.GetActivitiesByAthleteAsync(athleteId, pageSize, pageIndex);
            var activitiesCount = await _activityQueries.GetActivitiesByAthleteCountAsync(athleteId);
            var activityDtos = _mapper.Map<IList<ActivityForListDto>>(activities);
            var model = new PaginatedItemsViewModel<ActivityForListDto>(pageIndex, pageSize, activitiesCount, activityDtos);
            return model;
        }

        [HttpGet]
        [Route("activityById")]
        [ProducesResponseType(typeof(ActivityDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ActivityDto>> GetActivityByIdAsync(int activityId)
        {
            Console.WriteLine($"Called ActivityByIdAsync activityId={activityId}");
            var activity = await _activityQueries.GetActivityByIdAsync(activityId);
            var activityDto = _mapper.Map<ActivityDto>(activity);
            return activityDto;
        }
    }
}
