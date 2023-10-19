using Dapper;
using Microsoft.Data.SqlClient;
using System.Globalization;
using WebAPI.Common.Infrastructure.Log;
using WebAPI.Common.Model;

namespace WebAPI.Common.Infrastructure.DataSeed
{
    public class DataSeedAthletes : DataSeedEntityBase<Athlete>, IDataSeedEntity<Athlete>
    {
        private const string _TABLE_NAME = "athletes";
        private readonly string _connectionString = string.Empty;
        private static readonly string[] _RequiredHeaders = new string[] { "id", "name" };

        public DataSeedAthletes(string connectionString, string sourcePath, ILoggerService logger)
            : base(connectionString, sourcePath, logger, _TABLE_NAME, _RequiredHeaders)
        {
            _connectionString = connectionString;
        }
        public IEnumerable<Athlete> GetFromFile() => base.GetFromFile(_CreateAthlete);

        private static Athlete _CreateAthlete(string[] columns, string[] headers)
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

        public void InsertData(Athlete athlete)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            connection.Execute("SET IDENTITY_INSERT athletes ON");
            string sql = $"insert into athletes (Id, Name) values (@Id, @Name)";
            connection.Execute(sql, athlete);
        }
    }
}
