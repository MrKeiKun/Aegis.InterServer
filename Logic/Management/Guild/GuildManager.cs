using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aegis.Data.Repositories.Contracts;
using Aegis.Logic.Management.Contracts.Guild;
using Aegis.Logic.Management.Contracts.Guild.Classes;
using AutoMapper;

namespace Aegis.Logic.Management.Guild
{
    public class GuildManager : IGuildManager
    {
        private readonly ConcurrentDictionary<int, Contracts.Guild.Classes.Guild> _guilds;

        private readonly ICharacterRepository _characterRepository;

        public GuildManager(ICharacterRepository characterRepository)
        {
            _characterRepository = characterRepository;
            _guilds = new ConcurrentDictionary<int, Contracts.Guild.Classes.Guild>();
        }

        public int? GetGDIDByGID(int gid)
        {
            return _characterRepository.GetGDIDByGID(gid);
        }

        public Contracts.Guild.Classes.Guild GetGuild(int guildId)
        {
            if (guildId == 0)
            {
                return null;
            }

            var guild = _guilds.FirstOrDefault(x => x.Key == guildId);
            if (guild.Value == null)
            {
                var g = new Contracts.Guild.Classes.Guild
                {
                    GuildInfo = Mapper.Map<GuildInfo>(_characterRepository.GetGuildInfo(guildId)),
                    GuildNotice = Mapper.Map<GuildNotice>(_characterRepository.GetGuildNotice(guildId)),
                    GuildMInfo = Mapper.Map<IEnumerable<GuildMInfo>>(_characterRepository.GetGuildMInfo(guildId)),
                    GuildAllyInfo = Mapper.Map<IEnumerable<GuildAllyInfo>>(_characterRepository.GetGuildAllyInfo(guildId)),
                    GuildBanishInfo = Mapper.Map<IEnumerable<GuildBanishInfo>>(_characterRepository.GetGuildBanishInfo(guildId)),
                    GuildMPosition = Mapper.Map<IEnumerable<GuildMPosition>>(_characterRepository.GetGuildMPosition(guildId)),
                    GuildSkill = TransformGuildSkill(_characterRepository.GetGuildSkill(guildId))
                };

                _guilds.TryAdd(guildId, g);
            }

            return _guilds[guildId];
        }

        public IEnumerable<int> GuildAgit()
        {
            return _characterRepository.GetGuildAgit();
        }

        private GuildSkill TransformGuildSkill(Data.Repositories.Contracts.Classes.GuildSkill skill)
        {
            if (skill == null)
            {
                return null;
            }

            var ret = new GuildSkill { Point = skill.Point };
            using (var ms = new MemoryStream(skill.Skill))
            {
                using (var br = new BinaryReader(ms))
                {
                    var l = new List<SkillEntry>();
                    for (var i = 0; i < ms.Length / 4; i++)
                    {
                        var se = new SkillEntry
                        {
                            SkillId = br.ReadInt16(),
                            Level = br.ReadInt16()
                        };

                        l.Add(se);
                    }

                    ret.Skills = l.ToArray();
                }
            }

            return ret;
        }

        public void FreeGuild(int guildId)
        {
            Contracts.Guild.Classes.Guild g;
            _guilds.TryRemove(guildId, out g);
        }

        public void UpdateGuildMember(int guildId, int gid, int service, int memberExp, int level, int job)
        {
            _characterRepository.UpdateGuildMember(guildId, gid, service, memberExp, level, job);
        }
    }
}