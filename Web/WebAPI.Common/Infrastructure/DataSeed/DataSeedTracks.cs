using Dapper;
using Microsoft.Data.SqlClient;
using System.Globalization;
using WebAPI.Common.Infrastructure.Log;
using WebAPI.Common.Model;

namespace WebAPI.Common.Infrastructure.DataSeed
{
    public class DataSeedTracks : DataSeedEntityBase<Track>, IDataSeedEntity<Track>
    {
        private const string _TABLE_NAME = "tracks";
        private readonly string _connectionString = string.Empty;
        private static readonly string[] _RequiredHeaders = new string[] { "Id", "ActivityId", "Name" };

        public DataSeedTracks(string connectionString, string sourcePath, ILoggerService logger) : base(connectionString, sourcePath, logger,_TABLE_NAME, _RequiredHeaders)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Track> GetFromFile() => base.GetFromFile(_CreateTrack);

        private Track _CreateTrack(string[] columns, string[] headers)
        {
            Dictionary<int, Activity> activitiesLookup = _GetActivitiesLookup();
            if (columns.Count() != headers.Count())
            {
                throw new Exception($"column count '{columns.Count()}' not the same as headers count'{headers.Count()}'");
            }
            string activityIdString = _GetStringValue(columns, headers, "ActivityId");
            if (!int.TryParse(activityIdString, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out int activityId))
            {
                throw new Exception($"activityId={activityIdString}is not a valid int number");
            }
            if (!activitiesLookup.ContainsKey(activityId))
            {
                throw new Exception($"type={activityId} does not exist in activities");
            }

            string trackIdString = _GetStringValue(columns, headers, "id");
            if (!int.TryParse(trackIdString, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out int id))
            {
                throw new Exception($"trackId={activityIdString}is not a valid int number");
            }

            var track = new Track
            {
                Id = id,
                Activity = activitiesLookup[activityId],
                Name = _GetStringValue(columns, headers, "name")
            };
            return track;
        }

        private Dictionary<int, Activity> _GetActivitiesLookup()
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var activities = connection.Query<Activity>("select Id,Name from Activities");
            return activities.ToDictionary(a => a.Id);
        }

        public void InsertData(Track track)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            connection.Execute("SET IDENTITY_INSERT tracks ON");
            string sql = $"insert into tracks (Id,ActivityId,Name) values (@Id,@ActivityId,@Name)";
            connection.Execute(sql, new
            {
                track.Id,
                ActivityId = track.Activity.Id,
                track.Name
            });
        }

    }
}
