using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using WebAPI.Common.Infrastructure.Log;

namespace WebAPI.Common.Infrastructure
{
    public class DataSeed
    {
        private readonly string _connectionString = string.Empty;
        private readonly ILoggerService _logger;

        public DataSeed(string constr, string sourcePath, ILoggerService logger)
        {
            _connectionString = !string.IsNullOrWhiteSpace(constr) ? constr : throw new ArgumentNullException(nameof(constr));
            _logger = logger;
        }

        public async Task SeedAsync()
        {

        }
    }
}
