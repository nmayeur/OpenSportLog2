using Dapper;
using Microsoft.Data.SqlClient;
using System.Globalization;
using WebAPI.Common.Infrastructure.Log;
using WebAPI.Common.Model;

namespace WebAPI.Common.Infrastructure.DataSeed
{
    public class DataSeedTrackSegments : DataSeedEntityBase<TrackSegment>, IDataSeedEntity<TrackSegment>
    {
        private const string _TABLE_NAME = "tracksegments";
        private readonly string _connectionString = string.Empty;
        private static readonly string[] _RequiredHeaders = new string[] { "Id", "TrackId" };

        public DataSeedTrackSegments(string connectionString, string sourcePath, ILoggerService logger) : base(connectionString, sourcePath, logger, _TABLE_NAME, _RequiredHeaders)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<TrackSegment> GetFromFile() => base.GetFromFile(_CreateTrackSegment);

        private TrackSegment _CreateTrackSegment(string[] columns, string[] headers)
        {
            Dictionary<int, Track> tracksLookup = _GetTracksLookup();
            if (columns.Count() != headers.Count())
            {
                throw new Exception($"column count '{columns.Count()}' not the same as headers count'{headers.Count()}'");
            }
            string trackIdString = _GetStringValue(columns, headers, "TrackId");
            if (!int.TryParse(trackIdString, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out int trackId))
            {
                throw new Exception($"trackId={trackIdString}is not a valid int number");
            }
            if (!tracksLookup.ContainsKey(trackId))
            {
                throw new Exception($"type={trackId} does not exist in activities");
            }

            string trackSegmentIdString = _GetStringValue(columns, headers, "id");
            if (!int.TryParse(trackSegmentIdString, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out int id))
            {
                throw new Exception($"trackSegmentId={trackSegmentIdString}is not a valid int number");
            }

            var trackSegment = new TrackSegment
            {
                Id = id,
                Track = tracksLookup[trackId]
            };
            return trackSegment;
        }

        private Dictionary<int, Track> _GetTracksLookup()
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var tracks = connection.Query<Track>("select Id,Name from Tracks");
            return tracks.ToDictionary(a => a.Id);
        }

        public void InsertData(TrackSegment trackSegment)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            connection.Execute("SET IDENTITY_INSERT tracksegments ON");
            string sql = $"insert into tracksegments (Id,TrackId) values (@Id,@TrackId)";
            connection.Execute(sql, new
            {
                trackSegment.Id,
                TrackId = trackSegment.Track.Id
            });
        }

    }
}
