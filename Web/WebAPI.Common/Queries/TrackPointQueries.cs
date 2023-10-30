using Dapper;
using Microsoft.Data.SqlClient;
using WebAPI.Common.Model;

namespace WebAPI.Common.Queries
{
    public class TrackPointQueries : ITrackPointQueries
    {
        private string _connectionString = string.Empty;

        public TrackPointQueries(string constr)
        {
            _connectionString = !string.IsNullOrWhiteSpace(constr) ? constr : throw new ArgumentNullException(nameof(constr));
        }

        public async Task<IList<TrackPoint>> GetTrackPointsFromActivityIdAsync(int activityId)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var trackpoints = await connection.QueryAsync<TrackPoint>(
                $@"select tp.Id, tp.Latitude,tp.Longitude from TrackPoints tp
inner join TrackSegments ts on tp.TrackSegmentId=ts.Id
inner join Tracks t on ts.TrackId=t.Id
where t.ActivityId=@activityId order by Time",
                param: new { activityId });
            return trackpoints.ToList();
        }

    }
}
