using Aegis.Logic.Management.Contracts.Group.Classes;
using Aegis.Logic.Management.Contracts.Guild.Classes;
using AutoMapper;

namespace Aegis.Infrastructure.Mappings
{
    public static class AutoMapper
    {
        public static void Setup()
        {
            // Repository -> Management
            Mapper.CreateMap<Data.Repositories.Contracts.Classes.GuildInfo, GuildInfo>();
            Mapper.CreateMap<Data.Repositories.Contracts.Classes.GuildMInfo, GuildMInfo>();
            Mapper.CreateMap<Data.Repositories.Contracts.Classes.GuildMPosition, GuildMPosition>();
            Mapper.CreateMap<Data.Repositories.Contracts.Classes.GuildNotice, GuildNotice>();
            Mapper.CreateMap<Data.Repositories.Contracts.Classes.GuildAllyInfo, GuildAllyInfo>();
            Mapper.CreateMap<Data.Repositories.Contracts.Classes.GuildBanishInfo, GuildBanishInfo>();
            Mapper.CreateMap<Data.Repositories.Contracts.Classes.GroupInfo, GroupInfo>();
            Mapper.CreateMap<Data.Repositories.Contracts.Classes.GroupMember, GroupMember>();
        }
    }
}