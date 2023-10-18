using Dapper;
using Microsoft.Data.SqlClient;
using WebAPI.Common.Model;

namespace WebAPI.Common.Queries
{
    public class ActivityQueries : IActivityQueries
    {
        private string _connectionString = string.Empty;

        public ActivityQueries(string constr)
        {
            _connectionString = !string.IsNullOrWhiteSpace(constr) ? constr : throw new ArgumentNullException(nameof(constr));
        }

        public async Task<IList<Activity>> GetActivitiesByAthleteAsync(int athleteId, int pageSize, int pageIndex)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var activities = await connection.QueryAsync<Activity, Athlete, Activity>(
                $@"SELECT top {pageSize} ac.Id
      ,ac.OriginId
      ,ac.OriginSystem
      ,ac.Name
      ,ac.Location
      ,ac.Calories
      ,ac.Sport
      ,ac.Time
      ,ac.TimeSpan as TimeSpanTicks
      ,ac.Cadence
      ,ac.HeartRate
      ,ac.Power
      ,ac.Temperature
      ,ac.AthleteId
      ,at.Name
  FROM Activities ac inner join Athletes at on ac.athleteid=at.id
  where ac.AthleteId=@athleteId",
            (activity, athlete) =>
            {
                activity.Athlete = athlete;
                return activity;
            },
            param: new { pageSize, athleteId },
                splitOn: "AthleteId"
            );
            return activities.ToList();
        }
        public async Task<int> GetActivitiesByAthleteCountAsync(int athleteId)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var count = await connection.ExecuteScalarAsync<int>(
                @"select count(*) from Activities where athleteId=@athleteId"
                    , new { athleteId }
                );

            return count;

        }

        public async Task<Activity> GetActivityByIdAsync(int activityId)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var activity = await connection.QueryAsync<Activity, Athlete, Activity>(
                $@"SELECT ac.Id
      ,ac.OriginId
      ,ac.OriginSystem
      ,ac.Name
      ,ac.Location
      ,ac.Calories
      ,ac.Sport
      ,ac.Time
      ,ac.TimeSpan as TimeSpanTicks
      ,ac.Cadence
      ,ac.HeartRate
      ,ac.Power
      ,ac.Temperature
      ,ac.AthleteId
      ,at.Name
  FROM Activities ac inner join Athletes at on ac.athleteid=at.id
  where ac.Id=@activityId",
            (activity, athlete) =>
            {
                activity.Athlete = athlete;
                return activity;
            },
            param: new { activityId },
                splitOn: "AthleteId"
            );
            return activity.First();
        }
    }
}
