using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using Aegis.CrossCutting.Configuration.Contracts;
using Aegis.CrossCutting.GlobalDataClasses;
using Aegis.Data.Repositories.Contracts;
using Dapper;

namespace Aegis.Data.Repositories
{
    public class ScriptRepository : IScriptRepository
    {
        private readonly string _connectionString;

        public ScriptRepository(IConfigurator configurator)
        {
            _connectionString = $"FILEDSN={configurator.AppPath("script.dsn")};Uid=script;Pwd={configurator.Get<string>("database", "DatabasePassword").Decrypt()};";
        }

        public IEnumerable<ExperienceInfo> GetGuildExp()
        {
            using (var connection = new OdbcConnection(_connectionString))
            {
                return connection.Query<ExperienceInfo>("GetGuildExp", commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}