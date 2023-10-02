using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Model;
using WebAPI.Common.ViewModel;

namespace WebAPI.Common.Queries
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityQueries _activityQueries;

        public ActivityController(IActivityQueries activityQueries)
        {
            _activityQueries = activityQueries ?? throw new ArgumentNullException(nameof(activityQueries));
        }

        [HttpGet]
        [Route("activitiesByAthlete")]
        [ProducesResponseType(typeof(PaginatedItemsViewModel<Activity>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ActivitiesByAthleteAsync(int athleteId, [FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            var activities = await _activityQueries.GetActivitiesByAthleteAsync(athleteId, pageSize, pageIndex);
            var activitiesCount = await _activityQueries.GetActivitiesByAthleteCountAsync(athleteId);
            var model = new PaginatedItemsViewModel<Activity>(pageIndex, pageSize, activitiesCount, activities);
            return Ok(model);
        }
    }
}
