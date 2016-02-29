using System;
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
    public class CharacterRepository : ICharacterRepository
    {
        private readonly string _connectionString;
        
        public CharacterRepository(IConfigurator configurator)
        {
            _connectionString = $"FILEDSN={configurator.AppPath("character.dsn")};Uid=character;Pwd={configurator.Get<string>("database", "DatabasePassword").Decrypt()};";
        }

        public int? GetGDIDByGID(int gid)
        {
            using (var connection = new OdbcConnection(_connectionString))
            {
                return connection.Query<int?>("GetGDIDByGID ?", new { gid }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        public IEnumerable<MakerRank> GetTopMakerRank(int makerType)
        {
            using (var connection = new OdbcConnection(_connectionString))
            {
                return connection.Query<MakerRank>("GetTopMakerRank ?", new { makerType }, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public byte[] GetFriends(int gid)
        {
            using (var connection = new OdbcConnection(_connectionString))
            {
                return connection.Query<byte[]>("GetFriends ?", new { gid }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        public int? GetMember(int gid)
        {
            using (var connection = new OdbcConnection(_connectionString))
            {
                return connection.Query<int?>("GetMember ?", new { gid }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        public IEnumerable<GroupMember> GetGroupMembers(int groupId)
        {
            using (var connection = new OdbcConnection(_connectionString))
            {
                return connection.Query<GroupMember>("GetGroupMembers ?", new { groupId }, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public GroupInfo GetGroupInfo(int groupId)
        {
            using (var connection = new OdbcConnection(_connectionString))
            {
                return connection.Query<GroupInfo>("GetGroupInfo ?", new { groupId }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        public int? GetGroupID(string name)
        {
            using (var connection = new OdbcConnection(_connectionString))
            {
                return connection.Query<int?>("GetGroupID ?", new { name }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        public bool DeleteMember(int gid)
        {
            using (var connection = new OdbcConnection(_connectionString))
            {
                connection.Execute("DeleteMember ?", new { gid }, commandType: System.Data.CommandType.StoredProcedure);
                return true;
            }
        }

        public bool DeleteGroup(int groupId)
        {
            using (var connection = new OdbcConnection(_connectionString))
            {
                connection.Execute("DeleteGroup ?", new { groupId }, commandType: System.Data.CommandType.StoredProcedure);
                return true;
            }
        }

        public bool InsertGroup(string name, int groupOption)
        {
            using (var connection = new OdbcConnection(_connectionString))
            {
                connection.Execute("InsertGroup ?,?", new { name, groupOption }, commandType: System.Data.CommandType.StoredProcedure);
                return true;
            }
        }

        public bool InsertMember(int gid, int aid, string characterName, int groupId, int roleId)
        {
            using (var connection = new OdbcConnection(_connectionString))
            {
                connection.Execute("InsertMember ?,?,?,?,?", new { gid, aid, characterName, groupId, roleId }, commandType: System.Data.CommandType.StoredProcedure);
                return true;
            }
        }

        public int? GetGuildID(string name)
        {
            using (var connection = new OdbcConnection(_connectionString))
            {
                return connection.Query<int?>("GetGuildID ?", new { name }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        public IEnumerable<int> GetGuildAgit()
        {
            using (var connection = new OdbcConnection(_connectionString))
            {
                return connection.Query<int>("GetGuildAgitDB", commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public GuildInfo GetGuildInfo(int guildId)
        {
            using (var connection = new OdbcConnection(_connectionString))
            {
                return connection.Query<GuildInfo>("GetGuildInfoDB ?", new { guildId }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        GuildNotice ICharacterRepository.GetGuildNotice(int guildId)
        {
            using (var connection = new OdbcConnection(_connectionString))
            {
                return connection.Query<GuildNotice>("GetGuildNoticeDB ?", new { guildId }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        public IEnumerable<GuildMInfo> GetGuildMInfo(int guildId)
        {
            using (var connection = new OdbcConnection(_connectionString))
            {
                return connection.Query<GuildMInfo>("GetGuildMInfoDB ?", new { guildId }, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<GuildAllyInfo> GetGuildAllyInfo(int guildId)
        {
            using (var connection = new OdbcConnection(_connectionString))
            {
                return connection.Query<GuildAllyInfo>("GetGuildAllyInfo ?", new { guildId }, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<GuildBanishInfo> GetGuildBanishInfo(int guildId)
        {
            using (var connection = new OdbcConnection(_connectionString))
            {
                return connection.Query<GuildBanishInfo>("GetGuildBanishInfoDB ?", new { guildId }, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<GuildMPosition> GetGuildMPosition(int guildId)
        {
            using (var connection = new OdbcConnection(_connectionString))
            {
                return connection.Query<GuildMPosition>("GetGuildMPositionDB ?", new { guildId }, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public GuildSkill GetGuildSkill(int guildId)
        {
            using (var connection = new OdbcConnection(_connectionString))
            {
                return connection.Query<GuildSkill>("GetGuildSkill ?", new { guildId }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        public bool UpdateGuildMember(int guildId, int gid, int service, int exp, int level, int @class)
        {
            using (var connection = new OdbcConnection(_connectionString))
            {
                connection.Execute("UpdateGuildMember ?,?,?,?,?,?", new { guildId, gid, service, exp, level, @class }, commandType: System.Data.CommandType.StoredProcedure);
                return true;
            }
        }

        public bool UpdateGuildInfo(int guildId, int level, string name, string masterName, int maxUserNum, int honor, int virtue, int type, int @class, int money, int arenaWin, int arenaLose, int arenaDrawn, string manageLand, int exp, int emblem, int point, string desc)
        {
            using (var connection = new OdbcConnection(_connectionString))
            {
                connection.Execute("UpdateGuildInfo ?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?", new { guildId, level, name, masterName,maxUserNum, honor,virtue, type, @class, money, arenaWin, arenaLose, arenaDrawn, manageLand, exp, emblem, point, desc }, commandType: System.Data.CommandType.StoredProcedure);
                return true;
            }
        }

        public bool UpdateGuildSkill(int guildId, byte[] skills, int point)
        {
            using (var connection = new OdbcConnection(_connectionString))
            {
                connection.Execute("UpdateGuildSkill ?,?,?", new { guildId, skills, point }, commandType: System.Data.CommandType.StoredProcedure);
                return true;
            }
        }

        public bool InsertGuild(string name, string masterName)
        {
            using (var connection = new OdbcConnection(_connectionString))
            {
                connection.Execute("InsertGuild ?,?", new { name, masterName }, commandType: System.Data.CommandType.StoredProcedure);
                return true;
            }
        }

        public bool InsertGuildMInfo(int gid, string name, string accountName, int level, int @class, string memo, int service, int memberExp, int guildId, int aid, int positionId)
        {
            using (var connection = new OdbcConnection(_connectionString))
            {
                connection.Execute("InsertGuildMInfo ?,?,?,?,?,?,?,?,?,?,?", new { gid, name, accountName, level, @class, memo, service, memberExp, guildId, aid, positionId }, commandType: System.Data.CommandType.StoredProcedure);
                return true;
            }
        }

        public bool InsertGuildMPosition(int guildId, int ranking, string pos, int @join, int penalty, int positionId, int service)
        {
            using (var connection = new OdbcConnection(_connectionString))
            {
                connection.Execute("InsertGuildMPosition ?,?,?,?,?,?,?", new { guildId, ranking, pos, @join, penalty, positionId, service }, commandType: System.Data.CommandType.StoredProcedure);
                return true;
            }
        }

        public bool InsertGuildNotice(int guildId)
        {
            using (var connection = new OdbcConnection(_connectionString))
            {
                connection.Execute("InsertGuildNotice ?", new { guildId }, commandType: System.Data.CommandType.StoredProcedure);
                return true;
            }
        }

        public bool InsertGuildSkill(int guildId, int point)
        {
            using (var connection = new OdbcConnection(_connectionString))
            {
                connection.Execute("InsertGuildSkill ?, ?", new { guildId, point }, commandType: System.Data.CommandType.StoredProcedure);
                return true;
            }
        }
    }
}