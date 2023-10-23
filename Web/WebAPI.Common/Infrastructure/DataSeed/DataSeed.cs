using Dapper;
using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;
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

        public DataSeed(string connectionString, string sourcePath, ILoggerService logger)
        {
            _connectionString = !string.IsNullOrWhiteSpace(connectionString) ? connectionString : throw new ArgumentNullException(nameof(connectionString));
            _connectionStringMaster = Regex.Replace(connectionString, "Initial Catalog=([^;]*)", "Initial Catalog=master");
            _logger = logger;
            _sourcePath = sourcePath;
        }

        private async Task<bool> _UpdateTable<T>(IDataSeedEntity<T> dataSeedAthletes, bool withData = true)
        {
            if (!await dataSeedAthletes.CheckTableAsync())
            {
                dataSeedAthletes.CreateTable();
                if (withData)
                {
                    foreach (var athlete in dataSeedAthletes.GetFromFile())
                    {
                        dataSeedAthletes.InsertData(athlete);
                    }
                }
            }
            return true;
        }
        public async Task<bool> SeedAsync()
        {
            if (!await _CheckDatabase(DBNAME))
            {
                _CreateDatabase(DBNAME);
            }

            await _UpdateTable(new DataSeedAthletes(_connectionString, _sourcePath, _logger));
            await _UpdateTable(new DataSeedActivities(_connectionString, _sourcePath, _logger));
            await _UpdateTable(new DataSeedTracks(_connectionString, _sourcePath, _logger));
            await _UpdateTable(new DataSeedTrackSegments(_connectionString, _sourcePath, _logger));
            await _UpdateTable(new DataSeedTrackPoints(_connectionString, _sourcePath, _logger));

            return true;
        }

        public async Task<bool> SyncStructureAsync()
        {
            await _UpdateTable(new DataSeedAthletes(_connectionString, _sourcePath, _logger), false);
            await _UpdateTable(new DataSeedActivities(_connectionString, _sourcePath, _logger), false);
            await _UpdateTable(new DataSeedTracks(_connectionString, _sourcePath, _logger), false);
            await _UpdateTable(new DataSeedTrackSegments(_connectionString, _sourcePath, _logger), false);
            await _UpdateTable(new DataSeedTrackPoints(_connectionString, _sourcePath, _logger), false);

            return true;
        }


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