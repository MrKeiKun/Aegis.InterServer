using System;
using System.Collections.Generic;
using System.Data.Odbc;
using Aegis.CrossCutting.Configuration.Contracts;
using Aegis.CrossCutting.GlobalDataClasses;
using Aegis.Data.Repositories.Contracts;
using Dapper;

namespace Aegis.Data.Repositories
{
    public class IPInfoRepository : IIPInfoRepository
    {
        private readonly string _connectionString;

        public IPInfoRepository(IConfigurator configurator)
        {
            _connectionString = $"FILEDSN={configurator.AppPath("ipinfo.dsn")};Uid=ipinfo;Pwd={configurator.Get<string>("database", "DatabasePassword").Decrypt()};";
        }

        public IEnumerable<MapInfo> GetMapInfo()
        {
            using (var connection = new OdbcConnection(_connectionString))
            {
                return connection.Query<MapInfo>("select ZSID, MapName, MapID, type from MapInfo");
            }
        }
    }
}