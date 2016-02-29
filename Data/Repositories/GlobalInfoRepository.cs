using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using Aegis.CrossCutting.Configuration.Contracts;
using Aegis.CrossCutting.GlobalDataClasses;
using Aegis.Data.Repositories.Contracts;
using Aegis.Data.Repositories.Contracts.Classes;
using Dapper;

namespace Aegis.Data.Repositories
{
    public class GlobalInfoRepository : IGlobalInfoRepository
    {
        private readonly string _connectionString;

        public GlobalInfoRepository(IConfigurator configurator)
        {
            _connectionString = $"FILEDSN={configurator.AppPath("globalinfo.dsn")};Uid=globalinfo;Pwd={configurator.Get<string>("database", "DatabasePassword").Decrypt()};";
        }

        public IEnumerable<Server> GetServerInfo()
        {
            using (var connection = new OdbcConnection(_connectionString))
            {
                return connection.Query<Server>("GetServerInfo2",  commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}