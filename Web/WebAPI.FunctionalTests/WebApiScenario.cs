namespace WebAPI.FunctionalTests
{
    public class WebApiScenario: WebApiScenarioBase
    {
        [Fact]
        public async Task Get_get_all_catalogitems_and_response_ok_status_code()
        {
            using var server = CreateServer();
            var response = await server.CreateClient()
                .GetAsync(Get.ActivityById(1));

            response.EnsureSuccessStatusCode();
        }
    }
}
