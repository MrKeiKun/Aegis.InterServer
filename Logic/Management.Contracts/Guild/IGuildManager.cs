using System.Collections.Generic;

namespace Aegis.Logic.Management.Contracts.Guild
{
    public interface IGuildManager
    {
        int? GetGDIDByGID(int gid);
        Classes.Guild GetGuild(int guildId);

        IEnumerable<int> GuildAgit();

        void FreeGuild(int guildId);
        void UpdateGuildMember(int guildId, int gid, int service, int memberExp, int level, int job);
    }
}