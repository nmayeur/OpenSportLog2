using Microsoft.AspNetCore.Mvc.Testing;

namespace WebAPI.FunctionalTests
{
    public class WebApiScenarioBase
    {
        private class WebApiApplication : WebApplicationFactory<Program>
        {
            protected override IHost CreateHost(IHostBuilder builder)
            {
                builder.ConfigureAppConfiguration(c =>
                {
                    var directory = Path.GetDirectoryName(typeof(WebApiScenarioBase).Assembly.Location)!;

                    c.AddJsonFile(Path.Combine(directory, "appsettings.WebApi.json"), optional: false);
                });

                return base.CreateHost(builder);
            }
        }

        public TestServer CreateServer()
        {
            var factory = new WebApiApplication();
            return factory.Server;
        }

        public static class Get
        {
            public static string ActivityById(int activityId)
            {
                return $"api/activity/activityById?activityId={activityId}";
            }

        }
    }
}