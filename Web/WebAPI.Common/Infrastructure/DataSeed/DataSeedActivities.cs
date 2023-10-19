using Dapper;
using Microsoft.Data.SqlClient;
using System.Globalization;
using WebAPI.Common.Infrastructure.Log;
using WebAPI.Common.Model;

namespace WebAPI.Common.Infrastructure.DataSeed
{
    public class DataSeedActivities : DataSeedEntityBase<Activity>, IDataSeedEntity<Activity>
    {
        private const string _TABLE_NAME = "activities";
        private readonly string _connectionString = string.Empty;
        private static readonly string[] _RequiredHeaders = new string[] { "Id", "AthleteId", "OriginSystem", "Name", "Location", "Calories", "Sport", "Cadence", "HeartRate", "Power", "Temperature", "Time", "OriginId", "TimeSpan" };

        public DataSeedActivities(string connectionString, string sourcePath, ILoggerService logger) : base(connectionString, sourcePath, logger, _TABLE_NAME, _RequiredHeaders)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Activity> GetFromFile() => base.GetFromFile(_CreateActivity);

        private Activity _CreateActivity(string[] columns, string[] headers)
        {
            Dictionary<int, Athlete> athletesLookup = _GetAthletesLookup();
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

        private Dictionary<int, Athlete> _GetAthletesLookup()
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var athletes = connection.Query<Athlete>("select Id,Name from Athletes");
            return athletes.ToDictionary(a => a.Id);
        }

        public void InsertData(Activity activity)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            connection.Execute("SET IDENTITY_INSERT activities ON");
            string sql = $"insert into activities (Id,AthleteId,OriginSystem,Name,Location,Calories,Sport,Cadence,HeartRate,Power,Temperature,Time,OriginId,TimeSpan) values (@Id,@AthleteId,@OriginSystem,@Name,@Location,@Calories,@Sport,@Cadence,@HeartRate,@Power,@Temperature,@Time,@OriginId,@TimeSpanTicks)";
            connection.Execute(sql, new
            {
                activity.Id,
                AthleteId = activity.Athlete.Id,
                activity.OriginSystem,
                activity.Name,
                activity.Location,
                activity.Calories,
                activity.Sport,
                activity.Cadence,
                activity.HeartRate,
                activity.Power,
                activity.Temperature,
                activity.Time,
                activity.OriginId,
                activity.TimeSpanTicks
            });
        }
    }
}
