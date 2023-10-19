using Dapper;
using Microsoft.Data.SqlClient;
using System.Globalization;
using WebAPI.Common.Infrastructure.Log;
using WebAPI.Common.Model;

namespace WebAPI.Common.Infrastructure.DataSeed
{
    public class DataSeedTrackPoints : DataSeedEntityBase<TrackPoint>, IDataSeedEntity<TrackPoint>
    {
        private const string _TABLE_NAME = "trackpoints";
        private readonly string _connectionString = string.Empty;
        private static readonly string[] _RequiredHeaders = new string[] { "Id", "Latitude", "Longitude", "Elevation", "HeartRate", "Cadence", "TrackSegmentId", "Power", "Temperature", "Time" };

        public DataSeedTrackPoints(string connectionString, string sourcePath, ILoggerService logger) : base(connectionString, sourcePath, logger, _TABLE_NAME, _RequiredHeaders)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<TrackPoint> GetFromFile() => base.GetFromFile(_CreateTrackPoint);

        private TrackPoint _CreateTrackPoint(string[] columns, string[] headers)
        {
            Dictionary<int, TrackSegment> trackSegmentsLookup = _GetTrackSegmentsLookup();
            if (columns.Count() != headers.Count())
            {
                throw new Exception($"column count '{columns.Count()}' not the same as headers count'{headers.Count()}'");
            }
            string trackSegementIdString = _GetStringValue(columns, headers, "TrackSegmentId");
            if (!int.TryParse(trackSegementIdString, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out int trackSegmentId))
            {
                throw new Exception($"trackSegmentId={trackSegementIdString}is not a valid int number");
            }
            if (!trackSegmentsLookup.ContainsKey(trackSegmentId))
            {
                throw new Exception($"type={trackSegmentId} does not exist in track segments");
            }

            string trackPointIdString = _GetStringValue(columns, headers, "id");
            if (!int.TryParse(trackPointIdString, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out int id))
            {
                throw new Exception($"trackId={trackSegementIdString}is not a valid int number");
            }

            var track = new TrackPoint
            {
                Id = id,
                TrackSegment = trackSegmentsLookup[trackSegmentId],
                Latitude = _GetDecimalValue(columns, headers, "latitude"),
                Longitude = _GetDecimalValue(columns, headers, "longitude"),
                Elevation = _GetDecimalValue(columns, headers, "elevation"),
                HeartRate = _GetIntValue(columns, headers, "heartrate"),
                Cadence = _GetIntValue(columns, headers, "cadence"),
                Power = _GetIntValue(columns, headers, "power"),
                Temperature = _GetDecimalValue(columns, headers, "temperature"),
                Time = _GetDateTimeValue(columns, headers, "time")
            };
            return track;
        }

        private Dictionary<int, TrackSegment> _GetTrackSegmentsLookup()
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var trackSegements = connection.Query<TrackSegment>("select Id from TrackSegments");
            return trackSegements.ToDictionary(a => a.Id);
        }

        public void InsertData(TrackPoint trackPoint)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            connection.Execute("SET IDENTITY_INSERT TrackPoints ON");
            string sql = $@"INSERT INTO [dbo].[TrackPoints]
           (Id,[Latitude]
           ,[Longitude]
           ,[Elevation]
           ,[HeartRate]
           ,[Cadence]
           ,[TrackSegmentId]
           ,[Power]
           ,[Temperature]
           ,[Time])
     VALUES
           (@Id,@Latitude
           ,@Longitude
           ,@Elevation
           ,@HeartRate
           ,@Cadence
           ,@TrackSegmentId
           ,@Power
           ,@Temperature
           ,@Time)";
            connection.Execute(sql, new
            {
                trackPoint.Id,
                TrackSegmentId = trackPoint.TrackSegment.Id,
                trackPoint.Latitude,
                trackPoint.Longitude,
                trackPoint.Elevation,
                trackPoint.HeartRate,
                trackPoint.Cadence,
                trackPoint.Power,
                trackPoint.Temperature,
                trackPoint.Time
            });
        }

    }
}
