using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;
using WebAPI.Common.Extensions;
using WebAPI.Common.Infrastructure.Log;
using WebAPI.Common.Model;

namespace WebAPI.Common.Infrastructure.DataSeed
{
    public class DataSeed
    {
        private readonly string _connectionString = string.Empty;
        private readonly string _connectionStringMaster = string.Empty;
        private readonly ILoggerService _logger;
        private readonly string _sourcePath;
        private static readonly string DBNAME = "osl";
        private readonly DataSeedCommands _commands;

        public DataSeed(string connectionString, string sourcePath, ILoggerService logger, DataSeedCommands commands)
        {
            _connectionString = !string.IsNullOrWhiteSpace(connectionString) ? connectionString : throw new ArgumentNullException(nameof(connectionString));
            _connectionStringMaster = Regex.Replace(connectionString, "Initial Catalog=([^;]*)", "Initial Catalog=master");
            _logger = logger;
            _sourcePath = sourcePath;
            _commands = commands;
        }

        public async Task<bool> SeedAsync()
        {
            if (!await _CheckDatabase(DBNAME))
            {
                _CreateDatabase(DBNAME);
            }

            if (!await _CheckAthleteTable())
            {
                _CreateAthleteTable();
                foreach (var athlete in _GetAthletesFromFile())
                {
                    _commands.CreateAthlete(athlete);
                }
            }

            if (!await _CheckActivityTable())
            {
                _CreateActivityTable();
                foreach (var activity in _GetActivitiesFromFile())
                {
                    _commands.CreateActivity(activity);
                }
            }
            if (!await _CheckTrackTable())
            {
                _CreateTrackTable();
                foreach (var track in _GetTracksFromFile())
                {
                    _commands.CreateTrack(track);
                }
            }
            if (!await _CheckTrackSegmentTable())
            {
                _CreateTrackSegmentTable();
                foreach (var trackSegment in _GetTrackSegmentsFromFile())
                {
                    _commands.CreateTrackSegment(trackSegment);
                }
            }
            return true;
        }

        #region Athlete data
        private void _CreateAthleteTable()
        {
            string createAthleteFile = Path.Combine(_sourcePath, "Setup", "athletes.sql");

            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(File.ReadAllText(createAthleteFile), connection);
            command.ExecuteNonQuery();
        }

        private async Task<bool> _CheckAthleteTable()
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var count = await connection.QuerySingleAsync<int>($"select count(*) from information_schema.tables where table_name ='Athletes'");
            return count > 0;
        }

        private IEnumerable<Athlete> _GetAthletesFromFile()
        {
            string csvFileAthletes = Path.Combine(_sourcePath, "Setup", "athletes.csv");

            string[] csvheaders;
            try
            {
                string[] requiredHeaders = { "id", "name" };
                csvheaders = _GetHeaders(csvFileAthletes, requiredHeaders);
            }
            catch (Exception ex)
            {
                _logger.Error("Error reading CSV headers", ex);
                throw;
            }

            return File.ReadAllLines(csvFileAthletes)
                                        .Skip(1) // skip header row
                                        .Select(row => Regex.Split(row, ";(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"))
                                        .SelectTry(columns => _CreateAthlete(columns, csvheaders))
                                        .OnCaughtException(ex => { _logger.Error("Error creating athlete while seeding database", ex); return null; })
                                        .Where(x => x != null);
        }

        private Athlete _CreateAthlete(string[] columns, string[] headers)
        {
            if (columns.Count() != headers.Count())
            {
                throw new Exception($"column count '{columns.Count()}' not the same as headers count'{headers.Count()}'");
            }

            string athleteId = _GetStringValue(columns, headers, "id");
            if (!int.TryParse(athleteId, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out int id))
            {
                throw new Exception($"athleteId={athleteId}is not a valid int number");
            }

            var athlete = new Athlete
            {
                Id = id,
                Name = _GetStringValue(columns, headers, "name")
            };
            return athlete;
        }

        private Dictionary<int, Athlete> _GetAthletesLookup()
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var athletes = connection.Query<Athlete>("select Id,Name from Athletes");
            return athletes.ToDictionary(a => a.Id);
        }

        #endregion

        #region Activity data
        private void _CreateActivityTable()
        {
            string createActivityFile = Path.Combine(_sourcePath, "Setup", "activities.sql");

            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var tsql = File.ReadAllText(createActivityFile);
            foreach (var bloc in tsql.Split("GO"))
            {
                SqlCommand command = new SqlCommand(bloc, connection);
                command.ExecuteNonQuery();
            }
        }

        private async Task<bool> _CheckActivityTable()
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var count = await connection.QuerySingleAsync<int>($"select count(*) from information_schema.tables where table_name ='Activities'");
            return count > 0;
        }

        private IEnumerable<Activity> _GetActivitiesFromFile()
        {
            string csvFileActivities = Path.Combine(_sourcePath, "Setup", "activities.csv");

            string[] csvheaders;
            try
            {
                string[] requiredHeaders = { "Id", "AthleteId", "OriginSystem", "Name", "Location", "Calories", "Sport", "Cadence", "HeartRate", "Power", "Temperature", "Time", "OriginId", "TimeSpan" };
                csvheaders = _GetHeaders(csvFileActivities, requiredHeaders);
            }
            catch (Exception ex)
            {
                _logger.Error("Error reading CSV headers", ex);
                throw;
            }

            return File.ReadAllLines(csvFileActivities)
                                        .Skip(1) // skip header row
                                        .Select(row => Regex.Split(row, ";(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"))
                                        .SelectTry(columns => _CreateActivity(columns, csvheaders, _GetAthletesLookup()))
                                        .OnCaughtException(ex => { _logger.Error("Error creating activity while seeding database", ex); return null; })
                                        .Where(x => x != null);
        }

        private Activity _CreateActivity(string[] columns, string[] headers, Dictionary<int, Athlete> athletesLookup)
        {
            if (columns.Count() != headers.Count())
            {
                throw new Exception($"column count '{columns.Count()}' not the same as headers count'{headers.Count()}'");
            }
            string athleteIdString = _GetStringValue(columns, headers, "AthleteId");
            if (!int.TryParse(athleteIdString, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out int athleteId))
            {
                throw new Exception($"athleteId={athleteIdString}is not a valid int number");
            }
            if (!athletesLookup.ContainsKey(athleteId))
            {
                throw new Exception($"type={athleteId} does not exist in athletes");
            }

            string activityIdString = _GetStringValue(columns, headers, "id");
            if (!int.TryParse(activityIdString, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out int id))
            {
                throw new Exception($"activityId={activityIdString}is not a valid int number");
            }

            var activity = new Activity
            {
                Id = id,
                Athlete = athletesLookup[athleteId],
                OriginId = _GetStringValue(columns, headers, "originid"),
                OriginSystem = _GetStringValue(columns, headers, "originsystem"),
                Name = _GetStringValue(columns, headers, "name"),
                Location = _GetStringValue(columns, headers, "location"),
                Calories = _GetIntValue(columns, headers, "calories"),
                Sport = (ACTIVITY_SPORT)_GetIntValue(columns, headers, "sport"),
                Cadence = _GetIntValue(columns, headers, "cadence"),
                HeartRate = _GetIntValue(columns, headers, "heartrate"),
                Power = _GetIntValue(columns, headers, "power"),
                Temperature = _GetIntValue(columns, headers, "temperature"),
                Time = _GetDateTimeValue(columns, headers, "time"),
                TimeSpan = _GetTimeSpanValue(columns, headers, "timespan")
            };
            return activity;
        }

        private Dictionary<int, Activity> _GetActivitiesLookup()
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var activities = connection.Query<Activity>("select Id,Name from Activities");
            return activities.ToDictionary(a => a.Id);
        }
        #endregion

        #region Track data
        private void _CreateTrackTable()
        {
            string createTracksFile = Path.Combine(_sourcePath, "Setup", "tracks.sql");

            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var tsql = File.ReadAllText(createTracksFile);
            foreach (var bloc in tsql.Split("GO"))
            {
                SqlCommand command = new SqlCommand(bloc, connection);
                command.ExecuteNonQuery();
            }
        }

        private async Task<bool> _CheckTrackTable()
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var count = await connection.QuerySingleAsync<int>($"select count(*) from information_schema.tables where table_name ='Tracks'");
            return count > 0;
        }

        private IEnumerable<Track> _GetTracksFromFile()
        {
            string csvFileTracks = Path.Combine(_sourcePath, "Setup", "tracks.csv");

            string[] csvheaders;
            try
            {
                string[] requiredHeaders = { "Id", "ActivityId", "Name" };
                csvheaders = _GetHeaders(csvFileTracks, requiredHeaders);
            }
            catch (Exception ex)
            {
                _logger.Error("Error reading CSV headers", ex);
                throw;
            }

            return File.ReadAllLines(csvFileTracks)
                                        .Skip(1) // skip header row
                                        .Select(row => Regex.Split(row, ";(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"))
                                        .SelectTry(columns => _CreateTrack(columns, csvheaders, _GetActivitiesLookup()))
                                        .OnCaughtException(ex => { _logger.Error("Error creating track while seeding database", ex); return null; })
                                        .Where(x => x != null);
        }

        private Track _CreateTrack(string[] columns, string[] headers, Dictionary<int, Activity> activitiesLookup)
        {
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

        private Dictionary<int, Track> _GetTracksLookup()
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var tracks = connection.Query<Track>("select Id,Name from Tracks");
            return tracks.ToDictionary(a => a.Id);
        }
        #endregion

        #region TrackSegment data
        private void _CreateTrackSegmentTable()
        {
            string createTrackSegmentFile = Path.Combine(_sourcePath, "Setup", "tracksegments.sql");

            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var tsql = File.ReadAllText(createTrackSegmentFile);
            foreach (var bloc in tsql.Split("GO"))
            {
                SqlCommand command = new SqlCommand(bloc, connection);
                command.ExecuteNonQuery();
            }
        }

        private async Task<bool> _CheckTrackSegmentTable()
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var count = await connection.QuerySingleAsync<int>($"select count(*) from information_schema.tables where table_name ='Tracksegments'");
            return count > 0;
        }

        private IEnumerable<TrackSegment> _GetTrackSegmentsFromFile()
        {
            string csvFileTracks = Path.Combine(_sourcePath, "Setup", "tracksegments.csv");

            string[] csvheaders;
            try
            {
                string[] requiredHeaders = { "Id", "TrackId" };
                csvheaders = _GetHeaders(csvFileTracks, requiredHeaders);
            }
            catch (Exception ex)
            {
                _logger.Error("Error reading CSV headers", ex);
                throw;
            }

            return File.ReadAllLines(csvFileTracks)
                                        .Skip(1) // skip header row
                                        .Select(row => Regex.Split(row, ";(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"))
                                        .SelectTry(columns => _CreateTrackSegment(columns, csvheaders, _GetTracksLookup()))
                                        .OnCaughtException(ex => { _logger.Error("Error creating track segment while seeding database", ex); return null; })
                                        .Where(x => x != null);
        }

        private TrackSegment _CreateTrackSegment(string[] columns, string[] headers, Dictionary<int, Track> tracksLookup)
        {
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

        private Dictionary<int, TrackSegment> _GetTrackSegmentssLookup()
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var tracks = connection.Query<TrackSegment>("select Id from Tracksegments");
            return tracks.ToDictionary(a => a.Id);
        }
        #endregion

        #region utility functions

        private string _GetStringValue(string[] columns, string[] headers, string columnName)
        {
            var val = columns[Array.IndexOf(headers, columnName.ToLowerInvariant())].Trim('"').Trim();
            return val;
        }
        private int _GetIntValue(string[] columns, string[] headers, string columnName)
        {
            int.TryParse(_GetStringValue(columns, headers, columnName), out int val);
            return val;
        }
        private long _GetLongValue(string[] columns, string[] headers, string columnName)
        {
            long.TryParse(_GetStringValue(columns, headers, columnName), out long val);
            return val;
        }
        private DateTime _GetDateTimeValue(string[] columns, string[] headers, string columnName)
        {
            DateTime.TryParse(_GetStringValue(columns, headers, columnName), out DateTime val);
            return val;
        }
        private TimeSpan _GetTimeSpanValue(string[] columns, string[] headers, string columnName)
        {
            var val = TimeSpan.FromTicks(_GetLongValue(columns, headers, columnName));
            return val;
        }

        private string[] _GetHeaders(string csvfile, string[] requiredHeaders, string[] optionalHeaders = null)
        {
            string[] csvheaders = File.ReadLines(csvfile).First().ToLowerInvariant().Split(';');

            if (csvheaders.Count() < requiredHeaders.Count())
            {
                throw new Exception($"requiredHeader count '{requiredHeaders.Count()}' is bigger then csv header count '{csvheaders.Count()}' ");
            }

            if (optionalHeaders != null)
            {
                if (csvheaders.Count() > (requiredHeaders.Count() + optionalHeaders.Count()))
                {
                    throw new Exception($"csv header count '{csvheaders.Count()}'  is larger then required '{requiredHeaders.Count()}' and optional '{optionalHeaders.Count()}' headers count");
                }
            }

            foreach (var requiredHeader in requiredHeaders)
            {
                if (!csvheaders.Contains(requiredHeader.ToLowerInvariant()))
                {
                    throw new Exception($"does not contain required header '{requiredHeader}'");
                }
            }

            return csvheaders;
        }
        #endregion

        #region DDL
        private void _CreateDatabase(string dbName)
        {
            using var connection = new SqlConnection(_connectionStringMaster);
            connection.Open();
            SqlCommand command = new SqlCommand($"USE master; CREATE DATABASE {dbName}", connection);
            command.ExecuteNonQuery();
        }

        private async Task<bool> _CheckDatabase(string dbName)
        {
            using var connection = new SqlConnection(_connectionStringMaster);
            connection.Open();
            var databaseCount = await connection.QuerySingleAsync<int>($"USE master; SELECT count(*) FROM sys.databases WHERE Name = '{dbName}'");
            return databaseCount > 0;
        }
        #endregion
    }
}