using System.Collections.Generic;
using Aegis.CrossCutting.GlobalDataClasses;
using Aegis.Data.Repositories.Contracts.Classes;

namespace Aegis.Data.Repositories.Contracts
{
    public interface ICharacterRepository
    {
        int? GetGDIDByGID(int gid);

        IEnumerable<MakerRank> GetTopMakerRank(int makerType);

        // friends
        byte[] GetFriends(int gid);

        // group
        int? GetMember(int gid);
        IEnumerable<GroupMember> GetGroupMembers(int groupId);
        GroupInfo GetGroupInfo(int groupId);
        int? GetGroupID(string name);

        bool DeleteMember(int gid);
        bool DeleteGroup(int groupId);

        bool InsertGroup(string name, int groupOption);
        bool InsertMember(int gid, int aid, string characterName, int groupId, int roleId);

        // guild
        int? GetGuildID(string name);
        IEnumerable<int> GetGuildAgit();
        GuildInfo GetGuildInfo(int guildId);
        GuildNotice GetGuildNotice(int guildId);
        IEnumerable<GuildMInfo> GetGuildMInfo(int guildId);
        IEnumerable<GuildAllyInfo> GetGuildAllyInfo(int guildId);
        IEnumerable<GuildBanishInfo> GetGuildBanishInfo(int guildId);
        IEnumerable<GuildMPosition> GetGuildMPosition(int guildId);
        GuildSkill GetGuildSkill(int guildId);

        bool UpdateGuildMember(int guildId, int gid, int service, int exp, int level, int @class);

        bool UpdateGuildInfo(int guildId, int level, string name, string masterName, int maxUserNum,
            int honor, int virtue, int type, int @class, int money, int arenaWin, int arenaLose,
            int arenaDrawn, string manageLand, int exp, int emblem, int point, string desc);

        bool UpdateGuildSkill(int guildId, byte[] skills, int point);

        bool InsertGuild(string name, string masterName);
        bool InsertGuildMInfo(int gid, string name, string accountName, int level, int @class, string memo, int service, int memberExp, int guildId, int aid, int positionId);
        bool InsertGuildMPosition(int guildId, int ranking, string pos, int join, int penalty, int positionId, int service);
        bool InsertGuildNotice(int guildId);
        bool InsertGuildSkill(int guildId, int point);
    }
}