using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Model;
using WebAPI.ViewModel;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors]
    public class ActivityController : ControllerBase
    {
        [HttpGet]
        [Route("activitiesByAthlete")]
        [ProducesResponseType(typeof(PaginatedItemsViewModel<Activity>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ActivitiesByAthleteAsync(int athleteId, [FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            var athlete = new Athlete { Id = 1, Name = "Nicolas Mayeur" };
            var model = new PaginatedItemsViewModel<Activity>(pageIndex, pageSize, 10, new List<Activity> {
                new Activity
                {
                    Id=1,
                    Athlete=athlete,
                    Location="Rueil-Malmaison",
                    Sport=ACTIVITY_SPORT.RUNNING,
                    Time=DateTimeOffset.Now,
                    TimeSpan=TimeSpan.FromMinutes(45)
                },
                new Activity
                {
                    Id=2,
                    Athlete=athlete,
                    Location="Chevreuse",
                    Sport=ACTIVITY_SPORT.BIKING,
                    Time=DateTimeOffset.Now,
                    TimeSpan=TimeSpan.FromMinutes(244)
                },
                new Activity
                {
                    Id=3,
                    Athlete=athlete,
                    Location="Longchamp",
                    Sport=ACTIVITY_SPORT.BIKING,
                    Time=DateTimeOffset.Now,
                    TimeSpan=TimeSpan.FromMinutes(75)
                },
                new Activity
                {
                    Id=4,
                    Athlete=athlete,
                    Location="Rueil-Malmaison",
                    Sport=ACTIVITY_SPORT.BIKING,
                    Time=DateTimeOffset.Now,
                    TimeSpan=TimeSpan.FromMinutes(45)
                },
            });
            return Ok(model);
        }
    }
}
