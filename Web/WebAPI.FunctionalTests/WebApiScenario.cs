namespace WebAPI.FunctionalTests
{
    public class WebApiScenario: WebApiScenarioBase
    {
        [Fact]
        public async Task Get_get_activities_by_athlete_id_and_response_ok_status_code()
        {
            using var server = CreateServer();
            var response = await server.CreateClient()
                .GetAsync(Get.ActivitiesByAthleteId(1));

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Get_get_activities_by_id_and_response_ok_status_code()
        {
            using var server = CreateServer();
            var response = await server.CreateClient()
                .GetAsync(Get.ActivityById(1));

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Get_get_trackpoints_by_activity_id_and_response_ok_status_code()
        {
            using var server = CreateServer();
            var response = await server.CreateClient()
                .GetAsync(Get.TrackPointsByActivityId(1));

            response.EnsureSuccessStatusCode();
        }
    }
}
