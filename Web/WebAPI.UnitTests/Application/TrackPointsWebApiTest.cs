using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Common.Infrastructure.Log;
using WebAPI.Common.Model;
using WebAPI.Common.Queries;
using WebAPI.Common.Utils;
using WebAPI.Controllers;
using WebAPI.Dto;
using WebAPI.Dto.Dto;

namespace WebAPI.UnitTests.Application
{
    public class TrackPointsWebApiTest
    {
        private readonly Mock<ITrackPointQueries> _trackpointsQueriesMock;
        private readonly IMapper _mapper;

        public TrackPointsWebApiTest()
        {
            _trackpointsQueriesMock = new Mock<ITrackPointQueries>();

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            _mapper = mapperConfiguration.CreateMapper();
        }

        [Fact]
        public async Task Get_trackPointsByActivityId_success()
        {
            //Arrange
            var fakeActivityId = 123;
            IList<TrackPoint> fakeDynamicResult = new List<TrackPoint> {
                new TrackPoint {Latitude=(decimal)-122.48369693756104, Longitude=(decimal)37.83381888486939 },
                new TrackPoint {Latitude=(decimal)-122.48348236083984, Longitude=(decimal)37.83317489144141 },
                new TrackPoint {Latitude=(decimal)-122.48339653015138, Longitude=(decimal)37.83270036637107 }};
            _trackpointsQueriesMock.Setup(x => x.GetTrackPointsFromActivityIdAsync(fakeActivityId))
                .Returns(Task.FromResult(fakeDynamicResult));

            //Act
            var logger = new NLoggerService("WebApiTest");
            var trackController = new TrackController(_trackpointsQueriesMock.Object, _mapper, logger);
            var actionResult = await trackController.GetTrackPointsByActivityIdAsync(fakeActivityId);

            //Assert
            Assert.NotNull(actionResult.Value);
            for (var i = 0; i < actionResult.Value.Count; i++)
            {
                var expectedDto = _mapper.Map<TrackPointDto>(fakeDynamicResult[i]);
                Assert.Equal(expectedDto.Latitude, expectedDto.Latitude);
                Assert.Equal(expectedDto.Longitude, expectedDto.Longitude);
            }
        }
    }
}
