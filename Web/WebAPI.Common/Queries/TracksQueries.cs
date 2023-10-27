using Dapper;
using Microsoft.Data.SqlClient;
using WebAPI.Common.Model;

namespace WebAPI.Common.Queries
{
    public class TrackQueries : ITrackQueries
    {
        private string _connectionString = string.Empty;

        public TrackQueries(string constr)
        {
            _connectionString = !string.IsNullOrWhiteSpace(constr) ? constr : throw new ArgumentNullException(nameof(constr));
        }

        public async Task<IList<Track>> GetTracksByActivityAsync(int activityId, int pageSize, int pageIndex)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var activities = await connection.QueryAsync<Track, Activity, Track>(
                $@"SELECT top {pageSize} t.Id, t.Name, t.ActivityId
  FROM Tracks t inner join Activities ac on t.activityid=ac.id
  where ac.activityId=@activityId",
            (track, activity) =>
            {
                track.Activity = activity;
                return track;
            },
            param: new { pageSize, activityId },
                splitOn: "ActivityId"
            );
            return activities.ToList();
        }
        public async Task<int> GetTracksByActivityCountAsync(int activityId)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var count = await connection.ExecuteScalarAsync<int>(
                @"select count(*) from Tracks where activityid=@activityId"
                    , new { activityId }
                );

            return count;

        }

    }
}
