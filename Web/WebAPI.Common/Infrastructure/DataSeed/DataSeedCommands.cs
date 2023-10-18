﻿using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using WebAPI.Common.Model;
using static Azure.Core.HttpHeader;

namespace WebAPI.Common.Infrastructure.DataSeed
{
    public class DataSeedCommands
    {
        private readonly string _connectionString = string.Empty;
        public DataSeedCommands(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void CreateAthlete(Athlete athlete)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            connection.Execute("SET IDENTITY_INSERT athletes ON");
            string sql = $"insert into athletes (Id, Name) values (@Id, @Name)";
            connection.Execute(sql, athlete);
        }

        public void CreateActivity(Activity activity)
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
