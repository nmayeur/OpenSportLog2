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
    public class TrackController : ControllerBase
    {
        private readonly ITrackQueries _trackQueries;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;


        public TrackController(ITrackQueries trackQueries, IMapper mapper, ILoggerService logger)
        {
            _trackQueries = trackQueries ?? throw new ArgumentNullException(nameof(trackQueries));
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [Route("activitiesByAthlete")]
        [ProducesResponseType(typeof(PaginatedItemsViewModel<TrackForListDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PaginatedItemsViewModel<TrackForListDto>>> GetTracksByActivityAsync(int activityId, [FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            _logger.Debug($"Called ActivitiesByAthleteAsync athleteId={activityId}, pgeSize={pageSize}, pageIndex={pageIndex}");
            var activities = await _trackQueries.GetTracksByActivityAsync(activityId, pageSize, pageIndex);
            var activitiesCount = await _trackQueries.GetTracksByActivityCountAsync(activityId);
            var activityDtos = _mapper.Map<IList<TrackForListDto>>(activities);
            var model = new PaginatedItemsViewModel<TrackForListDto>(pageIndex, pageSize, activitiesCount, activityDtos);
            return model;
        }

    }
}
