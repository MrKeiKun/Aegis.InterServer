using Aegis.Logic.Management.Chat;
using Aegis.Logic.Management.Clan;
using Aegis.Logic.Management.Contracts.Chat;
using Aegis.Logic.Management.Contracts.Clan;
using Aegis.Logic.Management.Contracts.Experience;
using Aegis.Logic.Management.Contracts.Friend;
using Aegis.Logic.Management.Contracts.Group;
using Aegis.Logic.Management.Contracts.Guild;
using Aegis.Logic.Management.Contracts.Map;
using Aegis.Logic.Management.Contracts.MemorialDungeon;
using Aegis.Logic.Management.Contracts.Player;
using Aegis.Logic.Management.Contracts.Siege;
using Aegis.Logic.Management.Experience;
using Aegis.Logic.Management.Friend;
using Aegis.Logic.Management.Group;
using Aegis.Logic.Management.Guild;
using Aegis.Logic.Management.Map;
using Aegis.Logic.Management.MemorialDungeon;
using Aegis.Logic.Management.Player;
using Aegis.Logic.Management.Siege;
using Ninject.Modules;

namespace Aegis.Infrastructure.Mappings
{
    internal class ManagerMappings : NinjectModule
    {
        public override void Load()
        {
            Bind<IChatManager>().To<ChatManager>().InSingletonScope();
            Bind<IClanManager>().To<ClanManager>().InSingletonScope();
            Bind<IExperienceManager>().To<ExperienceManager>().InSingletonScope();
            Bind<IFriendManager>().To<FriendManager>().InSingletonScope();
            Bind<IGroupManager>().To<GroupManager>().InSingletonScope();
            Bind<IGuildManager>().To<GuildManager>().InSingletonScope();
            Bind<IMapManager>().To<MapManager>().InSingletonScope();
            Bind<IMemorialDungeonManager>().To<MemorialDungeonManager>().InSingletonScope();
            Bind<IPlayerManager>().To<PlayerManager>().InSingletonScope();
            Bind<ISiegeManager>().To<SiegeManager>().InSingletonScope();
        }
    }
}