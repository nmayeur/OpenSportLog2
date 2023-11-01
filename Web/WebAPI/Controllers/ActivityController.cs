using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Infrastructure.Log;
using WebAPI.Common.Queries;
using WebAPI.Common.ViewModel;
using WebAPI.Dto;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityQueries _activityQueries;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;


        public ActivityController(IActivityQueries activityQueries, IMapper mapper, ILoggerService logger)
        {
            _activityQueries = activityQueries ?? throw new ArgumentNullException(nameof(activityQueries));
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [Route("activitiesByAthleteId")]
        [ProducesResponseType(typeof(PaginatedItemsViewModel<ActivityForListDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PaginatedItemsViewModel<ActivityForListDto>>> GetActivitiesByAthleteIdAsync(int athleteId, [FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            _logger.Debug($"Called ActivitiesByAthleteIdAsync athleteId={athleteId}, pgeSize={pageSize}, pageIndex={pageIndex}");
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
            _logger.Debug($"Called ActivityByIdAsync activityId={activityId}");
            try
            {
                var activity = await _activityQueries.GetActivityByIdAsync(activityId);
                var activityDto = _mapper.Map<ActivityDto>(activity);
                return activityDto;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error calling ActivityByIdAsync activityId={activityId}", ex);
                throw;
            }
        }
    }
}
