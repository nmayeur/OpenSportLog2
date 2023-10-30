using AutoMapper;
using Moq;
using WebAPI.Common.Infrastructure.Log;
using WebAPI.Common.Model;
using WebAPI.Common.Queries;
using WebAPI.Common.Utils;
using WebAPI.Controllers;
using WebAPI.Dto;

namespace WebAPI.UnitTests.Application
{
    public class ActivitiesWebApiTest
    {
        private readonly Mock<IActivityQueries> _activityQueriesMock;
        private readonly IMapper _mapper;

        public ActivitiesWebApiTest()
        {
            _activityQueriesMock = new Mock<IActivityQueries>();

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            _mapper = mapperConfiguration.CreateMapper();
        }

        [Fact]
        public async Task Get_activityById_success()
        {
            //Arrange
            var fakeActivityId = 123;
            var fakeDynamicResult = new Activity { Id = fakeActivityId };
            _activityQueriesMock.Setup(x => x.GetActivityByIdAsync(fakeActivityId))
                .Returns(Task.FromResult(fakeDynamicResult));

            //Act
            var logger = new NLoggerService("WebApiTest");
            var activityController = new ActivityController(_activityQueriesMock.Object, _mapper, logger);
            var actionResult = await activityController.GetActivityByIdAsync(fakeActivityId);

            //Assert
            var expectedDto = _mapper.Map<ActivityDto>(fakeDynamicResult);
            Assert.NotNull(actionResult.Value);
            Assert.Equal(expectedDto.Id, actionResult.Value.Id);
        }

        [Fact]
        public async Task Get_activityByAthlete_success()
        {
            //Arrange
            var fakeAthleteId = 123;
            IList<Activity> fakeActivities = new List<Activity> {new Activity
            {
                Id = 444,
                Athlete = new Athlete { Id = fakeAthleteId }
            },new Activity
            {
                Id = 333,
                Athlete = new Athlete { Id = fakeAthleteId }
            } };
            _activityQueriesMock.Setup(x => x.GetActivitiesByAthleteAsync(fakeAthleteId, 10, 0))
                .Returns(Task.FromResult(fakeActivities));
            _activityQueriesMock.Setup(x => x.GetActivitiesByAthleteCountAsync(fakeAthleteId))
                .Returns(Task.FromResult(fakeActivities.Count));

            //Act
            var logger = new NLoggerService("WebApiTest");
            var activityController = new ActivityController(_activityQueriesMock.Object, _mapper, logger);
            var actionResult = await activityController.GetActivitiesByAthleteIdAsync(fakeAthleteId);

            //Assert
            var expectedDtos = _mapper.Map<IList<ActivityDto>>(fakeActivities);
            Assert.NotNull(actionResult.Value);
            Assert.Equal(2, actionResult.Value.Count);
            Assert.NotNull(actionResult.Value.Data);
            Assert.Equal(2, actionResult.Value.Data.Count());
            Assert.NotNull(actionResult.Value.Data.Where(a => a.Id == 333).First());
            Assert.NotNull(actionResult.Value.Data.Where(a => a.Id == 444).First());
        }
    }
}
