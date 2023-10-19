using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using WebAPI.Common.Extensions;
using WebAPI.Common.Infrastructure.Log;
using WebAPI.Common.Model;

namespace WebAPI.Common.Infrastructure.DataSeed
{
    public abstract class DataSeedEntityBase<T>
    {
        private readonly string _ConnectionString = string.Empty;
        private readonly ILoggerService _Logger;
        private readonly string _SourcePath;
        private readonly string _TableName;
        private readonly string[] _RequiredHeaders;

        public delegate void InsertDataDelegate(T data);
        public delegate T MapEntityFromColumns(string[] columns, string[] csvHeaders);

        public DataSeedEntityBase(string connectionString, string sourcePath, ILoggerService logger, string tableName, string[] requiredHeaders)
        {
            _ConnectionString = connectionString;
            _SourcePath = sourcePath;
            _Logger = logger;
            _TableName = tableName;
            _RequiredHeaders = requiredHeaders;
        }

        public void CreateTable()
        {
            string createTracksFile = Path.Combine(_SourcePath, "Setup", $"{_TableName}.sql");

            using var connection = new SqlConnection(_ConnectionString);
            connection.Open();
            var tsql = File.ReadAllText(createTracksFile);
            foreach (var bloc in tsql.Split("GO"))
            {
                SqlCommand command = new SqlCommand(bloc, connection);
                command.ExecuteNonQuery();
            }
        }

        public async Task<bool> CheckTableAsync()
        {
            using var connection = new SqlConnection(_ConnectionString);
            connection.Open();

            var count = await connection.QuerySingleAsync<int>($"select count(*) from information_schema.tables where table_name ='{_TableName}'");
            return count > 0;
        }

        public IEnumerable<T> GetFromFile(MapEntityFromColumns mapEntityFromColumnsDelegate)
        {
            string csvFile = Path.Combine(_SourcePath, "Setup", $"{_TableName}.csv");

            string[] csvheaders;
            try
            {
                csvheaders = _GetHeaders(csvFile, _RequiredHeaders);
            }
            catch (Exception ex)
            {
                _Logger.Error("Error reading CSV headers", ex);
                throw;
            }

            return File.ReadAllLines(csvFile)
                                        .Skip(1) // skip header row
                                        .Select(row => Regex.Split(row, ";(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"))
                                        .SelectTry(columns => mapEntityFromColumnsDelegate(columns, csvheaders))
                                        .OnCaughtException(ex => { _Logger.Error("Error creating track segment while seeding database", ex); return default(T); })
                                        .Where(x => x != null);
        }

        protected static string _GetStringValue(string[] columns, string[] headers, string columnName)
        {
            var val = columns[Array.IndexOf(headers, columnName.ToLowerInvariant())].Trim('"').Trim();
            return val;
        }
        protected static int _GetIntValue(string[] columns, string[] headers, string columnName)
        {
            int.TryParse(_GetStringValue(columns, headers, columnName), out int val);
            return val;
        }
        protected static long _GetLongValue(string[] columns, string[] headers, string columnName)
        {
            long.TryParse(_GetStringValue(columns, headers, columnName), out long val);
            return val;
        }
        protected static DateTime _GetDateTimeValue(string[] columns, string[] headers, string columnName)
        {
            DateTime.TryParse(_GetStringValue(columns, headers, columnName), out DateTime val);
            return val;
        }
        protected static TimeSpan _GetTimeSpanValue(string[] columns, string[] headers, string columnName)
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
    }
}
