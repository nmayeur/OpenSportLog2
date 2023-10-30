using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Infrastructure.Log;
using WebAPI.Common.Queries;
using WebAPI.Dto.Dto;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors]
    public class TrackController : ControllerBase
    {
        private readonly ITrackPointQueries _trackPointQueries;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;


        public TrackController(ITrackPointQueries trackPointQueries, IMapper mapper, ILoggerService logger)
        {
            _trackPointQueries = trackPointQueries ?? throw new ArgumentNullException(nameof(trackPointQueries));
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [Route("trackPointsByActivityId")]
        [ProducesResponseType(typeof(IList<TrackPointDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IList<TrackPointDto>>> GetTrackPointsByActivityIdAsync([FromQuery] int activityId)
        {
            _logger.Debug($"Called GetTrackPointsByActivityIdAsync activityId={activityId}");
            var trackPoints = await _trackPointQueries.GetTrackPointsFromActivityIdAsync(activityId);
            var trackPointsDto = _mapper.Map<IList<TrackPointDto>>(trackPoints);
            return new ActionResult<IList<TrackPointDto>>(trackPointsDto);
        }

    }
}
