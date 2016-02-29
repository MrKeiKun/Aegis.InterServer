using Aegis.CrossCutting.GlobalDataClasses;
using Aegis.CrossCutting.Network;
using Aegis.CrossCutting.Network.Classes;
using Aegis.CrossCutting.Network.Packets;
using Aegis.CrossCutting.Network.Packets.IZ;
using Aegis.CrossCutting.Network.Packets.ZI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegis.Logic.Management.Contracts.Chat;
using Aegis.Logic.Management.Contracts.Clan;
using Aegis.Logic.Management.Contracts.Friend;
using Aegis.Logic.Management.Contracts.Group;
using Aegis.Logic.Management.Contracts.Guild;
using Aegis.Logic.Management.Contracts.MemorialDungeon;
using Aegis.Logic.Management.Contracts.Player;
using Aegis.Logic.Management.Contracts.Player.Classes;
using Aegis.Logic.Management.Contracts.Siege;

namespace Aegis.Services.InterServer.Classes
{
    public class ZoneListener : RagnarokListener
    {
        private IChatManager _chatManager;
        private IClanManager _clanManager;
        private IFriendManager _friendManager;
        private IGroupManager _groupManager;
        private IGuildManager _guildManager;
        private IMemorialDungeonManager _memorialDungeonManager;
        private IPlayerManager _playerManager;
        private ISiegeManager _siegeManager;

        public ZoneListener(IChatManager chatManager, IClanManager clanManager, IFriendManager friendManager,
            IGroupManager groupManager, IGuildManager guildManager, IMemorialDungeonManager memorialDungeonManager,
            IPlayerManager playerManager, ISiegeManager siegeManager)
        {
            _chatManager = chatManager;
            _clanManager = clanManager;
            _friendManager = friendManager;
            _groupManager = groupManager;
            _guildManager = guildManager;
            _memorialDungeonManager = memorialDungeonManager;
            _siegeManager = siegeManager;
            _playerManager = playerManager;
        }

        protected override BaseClient CreateClient()
        {
            return new ZoneClient() { Owner = this };
        }

        public static ConcurrentDictionary<int, ZoneClient> Clients = new ConcurrentDictionary<int, ZoneClient>();
        public override void RemoveClient(BaseClient client, bool notifyAccount)
        {
            var z = (ZoneClient)client;
            ZoneClient c = null;
            Clients.TryRemove(z.Sid, out c);
        }

        private static void AddClient(ZoneClient client)
        {
            Clients.TryAdd(client.Sid, client);
        }

        private ZoneClient GetLowestResourceZone()
        {
            return Clients.OrderBy(x => x.Value.UsedKBytesMemory).FirstOrDefault().Value;
        }

        private void ZonePacket(int zsid, PacketBase packet)
        {
            var client = Clients.FirstOrDefault(x => x.Value.Sid == zsid).Value;
            client.EnqueuePacket(packet);
        }

        private void BroadcastPacket(PacketBase packet)
        {
            foreach (var client in Clients)
            {
                client.Value.EnqueuePacket(packet);
            }
        }

        protected override bool OnPacket(BaseClient client, PacketBase packet)
        {
            var zone = (ZoneClient)client;
            switch (packet.Command)
            {
                case (ushort)PACKET_COMMAND.ZI_AUTH_REQ:
                    return OnAuthRequest(client as ZoneClient, packet as ZI_AUTH_REQ);

                case (ushort)PACKET_COMMAND.ZI_VERSION:
                    return OnVersion(client as ZoneClient, packet as ZI_VERSION);

                case (ushort)PACKET_COMMAND.ZI_PING_LIVE:
                    return OnPingLive(client as ZoneClient, packet as ZI_PING_LIVE);

                case (ushort)PACKET_COMMAND.ZI_STATEINFO:
                    return OnStateInfo(client as ZoneClient, packet as ZI_STATEINFO);

                case (ushort)PACKET_COMMAND.ZI_LOGON:
                    return OnLogin(client as ZoneClient, packet as ZI_LOGON);

                case (ushort)PACKET_COMMAND.ZI_EXIT:
                    return OnExit(client as ZoneClient, packet as ZI_EXIT);

                case (ushort)PACKET_COMMAND.ZI_MAKE_GROUP2:
                    return OnMakeGroup2(client as ZoneClient, packet as ZI_MAKE_GROUP2);

                case (ushort)PACKET_COMMAND.ZI_REQ_LEAVE_GROUP:
                    return OnReqLeaveGroup(client as ZoneClient, packet as ZI_REQ_LEAVE_GROUP);

                case (ushort)PACKET_COMMAND.ZI_REQ_MAKE_GUILD:
                    return OnReqMakeGuild(client as ZoneClient, packet as ZI_REQ_MAKE_GUILD);

                case (ushort)PACKET_COMMAND.ZI_REQ_USER_CLANINFO:
                    return OnReqUserClanInfo(client as ZoneClient, packet as ZI_REQ_USER_CLANINFO);

                case (ushort)PACKET_COMMAND.ZI_WHISPER:
                    return OnWhisper(client as ZoneClient, packet as ZI_WHISPER);

                case (ushort)PACKET_COMMAND.ZI_ACK_WHISPER:
                    return OnAckWhisper(client as ZoneClient, packet as ZI_ACK_WHISPER);

                case (ushort)PACKET_COMMAND.ZI_BROADCAST:
                    return OnBroadcast(client as ZoneClient, packet as ZI_BROADCAST);

                case (ushort)PACKET_COMMAND.ZI_GUILD_CHAT:
                    return OnGuildChat(client as ZoneClient, packet as ZI_GUILD_CHAT);

                case (ushort)PACKET_COMMAND.ZI_REQ_USER_COUNT:
                    return OnReqUserCount(client as ZoneClient, packet as ZI_REQ_USER_COUNT);

                case (ushort)PACKET_COMMAND.ZI_MOVE_SERVER:
                    return OnMoveServer(client as ZoneClient, packet as ZI_MOVE_SERVER);

                case (ushort)PACKET_COMMAND.ZI_MAPMOVE:
                    return OnMapMove(client as ZoneClient, packet as ZI_MAPMOVE);

                case (ushort)PACKET_COMMAND.ZI_REQ_JOIN_CLAN:
                    return OnReqJoinClan(client as ZoneClient, packet as ZI_REQ_JOIN_CLAN);

                case (ushort)PACKET_COMMAND.ZI_REQ_BAN_GUILD:
                    return OnReqBanGuild(client as ZoneClient, packet as ZI_REQ_BAN_GUILD);

                case (ushort)PACKET_COMMAND.ZI_GUILD_NOTICE:
                    return OnGuildNotice(client as ZoneClient, packet as ZI_GUILD_NOTICE);

                case (ushort)PACKET_COMMAND.ZI_GDSKILL_UPDATE:
                    return OnGDSkillUpdate(client as ZoneClient, packet as ZI_GDSKILL_UPDATE);

                case (ushort)PACKET_COMMAND.ZI_INSTANTMAP_ALLOW:
                    return OnInstantMapAllow(client as ZoneClient, packet as ZI_INSTANTMAP_ALLOW);

                case (ushort)PACKET_COMMAND.ZI_MEMORIALDUNGEON_SUBSCRIPTION2:
                    return OnMemorialDungeonSubscription2(client as ZoneClient, packet as ZI_MEMORIALDUNGEON_SUBSCRIPTION2);

                case (ushort)PACKET_COMMAND.ZI_INSTANTMAP_CREATE_RES:
                    return OnInstantMapCreateRes(client as ZoneClient, packet as ZI_INSTANTMAP_CREATE_RES);
                default:
                    Logger.Error($"Packet not processed: {(PACKET_COMMAND)packet.Command}");
                    return true;
            }

            return true;
        }

        private bool OnAuthRequest(ZoneClient client, ZI_AUTH_REQ packet)
        {
            Logger.Debug($"ZSRV({client.IP().ToString()}) auth success");
            client.Sid = packet.ZSID;
            client.EnqueuePacket(new IZ_AUTH_ACK());
            client.EnqueuePacket(new IZ_REQ_EDIT_EXP { MonitorNum = 0, Death = 0, Drop = 0, Exp = 0 });
            Logger.Debug($"ZSRV({client.Sid}) auth... complete");
            return true;
        }

        private bool OnVersion(ZoneClient client, ZI_VERSION packet)
        {
            Logger.Debug($"ZSRV({client.Sid}) ZI_VERSION Version={packet.Version}");
            return true;
        }

        private bool OnPingLive(ZoneClient client, ZI_PING_LIVE packet)
        {
            client.EnqueuePacket(new IZ_PING_LIVE());
            return true;
        }

        private bool OnStateInfo(ZoneClient client, ZI_STATEINFO packet)
        {
            Logger.Debug($"ZSRV({client.Sid}) ZI_STATEINFO NumTotalNPC={packet.NumTotalNPC} UsedKBytesMemory={packet.UsedKBytesMemory}");
            client.UsedKBytesMemory = packet.UsedKBytesMemory;
            client.NumTotalNPC = packet.NumTotalNPC;
            return true;
        }

        private bool OnLogin(ZoneClient client, ZI_LOGON packet)
        {
            Logger.Debug($"ZSRV({client.Sid}) ZI_LOGON AID={packet.AID} AccountName={packet.AccountName} GID={packet.GID}, CharacterName={packet.CharacterName}, MapName {packet.MapName}");
            var myPlayers = _playerManager.FindPlayersByAID(packet.AID);
            if (myPlayers.Any())
            {
                client.EnqueuePacket(new IZ_DISCONNECT_CHARACTER { AID = packet.AID });
                return ExitPlayer(client, packet.GID, packet.AID);
            }

            // create the player trying to log in
            var myPlayer = new Player()
            {
                AID = packet.AID,
                GID = packet.GID,
                AccountName = packet.AccountName,
                CharacterName = packet.CharacterName,
                Head = packet.Head,
                HeadPalette = packet.HeadPalette,
                Level = packet.Level,
                Sex = (short)packet.Sex,
                Job = packet.Job,
                MapName = packet.MapName,
                ZSID = client.Sid,
                Status = 1
            };
            _playerManager.AddPlayer(myPlayer);

            var gdid = _guildManager.GetGDIDByGID(packet.GID);
            if (gdid.HasValue)
            {
                // if user has a guild
                myPlayer.GDID = gdid.Value;

                // GuildInfo
                var g = _guildManager.GetGuild(gdid.Value);
                var gi = g.GuildInfo;
                client.EnqueuePacket(new IZ_GUILDINFO_TOD
                {
                    AID = 0,
                    GDID = gdid.Value,
                    Data = new GUILDINFO(gi.GDID, gi.Level, gi.Name, gi.MName, gi.MaxUserNum, gi.UserNum, gi.Honor, gi.Virtue,
                        gi.Type, gi.Class, gi.Money, gi.ArenaWin, gi.ArenaLose, gi.ArenaDrawn, gi.ManageLand,
                        gi.Exp, gi.EmblemVersion, gi.Point, gi.Desc)
                });

                // GuildNotice
                client.EnqueuePacket(new IZ_GUILD_NOTICE
                {
                    GDID = gdid.Value,
                    Notice = g.GuildNotice.Notice,
                    Subject = g.GuildNotice.Subject
                });

                // GuildMemberInfo
                var gmi = g.GuildMInfo.ToArray();
                foreach (var player in gmi)
                {
                    var pc = _playerManager.FindPlayerByGID(player.GID);
                    if (pc == null) continue;

                    player.Status = pc.Status;
                    player.Sex = pc.Sex;
                    player.Job = pc.Job;
                    player.Head = pc.Head;
                    player.HeadPalette = pc.HeadPalette;
                }

                client.EnqueuePacket(new IZ_GUILD_MEMBERINFO_TOD
                {
                    AID = 0,
                    GDID = gdid.Value,
                    GuildMInfo = gmi.Select(x => new GUILDMINFO(x.GID, x.CharName, x.AccountName, x.Level, x.Memo, x.Service,
                        x.MemberExp, x.GDID, x.AID, x.PositionID, x.Head, x.HeadPalette, x.Sex, x.Job, x.Status)).ToArray()
                });

                // GuildAllyInfo
                client.EnqueuePacket(new IZ_GUILD_ALLYINFO_TOD
                {
                    AID = 0,
                    GDID = gdid.Value,
                    GuildAllyInfo = g.GuildAllyInfo.Select(x => new GUILDALLYINFO(x.GDID, x.OpponentGDID, x.GuildName, x.Relation)).ToArray()
                });

                // GuildBanishInfo
                client.EnqueuePacket(new IZ_GUILD_BANISHINFO_TOD
                {
                    AID = 0,
                    GDID = gdid.Value,
                    GuildBanishInfo = g.GuildBanishInfo.Select(x => new GUILDBANISHINFO(x.GDID, x.MemberName, string.Empty, x.Reason, x.GID, x.AID)).ToArray()
                });

                // GuildMPosition
                client.EnqueuePacket(new IZ_GUILD_MPOSITION_TOD
                {
                    AID = 0,
                    GDID = gdid.Value,
                    GuildMPosition = g.GuildMPosition.Select(x => new GUILDMPOSITION(x.GDID, x.Grade, x.PosName, x.JoinRight, x.PenaltyRight, x.PositionID, x.Service)).ToArray()
                });

                // GuildSkill
                client.EnqueuePacket(new IZ_GUILD_NOTIFYSKILLDATA
                {
                    IsForceSend = 1,
                    SkillPoint = g.GuildSkill.Point,
                    GuildSkill = g.GuildSkill.Skills.Select(x => new GUILDSKILL(x.SkillId, x.Level)).ToArray(),
                    GDID = gdid.Value,
                });

                BroadcastPacket(new IZ_UPDATE_CHARSTAT
                {
                    GDID = gdid.Value,
                    GID = packet.GID,
                    AID = packet.AID,
                    Status = myPlayer.Status,
                    Sex = myPlayer.Sex,
                    Head = myPlayer.Head,
                    HeadPalette = myPlayer.HeadPalette,
                    Job = (short)myPlayer.Job,
                    Level = myPlayer.Level
                });

                client.EnqueuePacket(new IZ_UPDATE_CHARGDID
                {
                    Type = 2,
                    GDID = gdid.Value,
                    EmblemVer = gi.EmblemVersion,
                    InterSID = 701348095,
                    GID = packet.GID,
                    AID = packet.AID,
                    Right = 17,
                    IsMaster = gi.MName == myPlayer.CharacterName,
                    GuildName = gi.Name
                });
            }

            client.EnqueuePacket(new IZ_ACK_LOGON { AID = packet.AID, GID = packet.GID, Type = 0 });

            // finished guild, group coming next
            var grid = _groupManager.GetMember(packet.GID);
            if (grid.HasValue)
            {
                var charinfo = new List<CHARINFO_IN_GROUP>();
                var g = _groupManager.GetGroup(grid.Value);
                foreach (var m in g.GroupMember)
                {
                    var p = _playerManager.FindPlayerByGID(m.GID);
                    if (p != null && p.Status == 1)
                    {
                        ZonePacket(p.ZSID, new IZ_ADD_MEMBER_TO_GROUP2
                        {
                            ReceiverAID = m.AID,
                            AID = packet.AID,
                            Role = 0,
                            State = 0,
                            GroupName = g.GroupInfo.GroupName,
                            CharacterName = packet.CharacterName,
                            MapName = packet.MapName,
                            ItemPickupRule = g.GroupInfo.ItemPickupRule,
                            ItemDivisionRule = g.GroupInfo.ItemDivisionRule
                        });

                        client.EnqueuePacket(new IZ_ADD_MEMBER_TO_GROUP2
                        {
                            ReceiverAID = packet.AID,
                            AID = m.AID,
                            Role = 0,
                            State = (byte)(p?.Status ?? 1),
                            GroupName = g.GroupInfo.GroupName,
                            CharacterName = m.CharName,
                            MapName = p?.MapName,
                            ItemPickupRule = g.GroupInfo.ItemPickupRule,
                            ItemDivisionRule = g.GroupInfo.ItemDivisionRule
                        });
                    }

                    byte status;
                    if (p == null || p.Status == 0)
                    {
                        status = 1;
                    }
                    else
                    {
                        status = 0;
                    }

                    Logger.Debug($"CHARINFO {m.CharName}, Status {status}");
                    charinfo.Add(new CHARINFO_IN_GROUP(m.AID, m.CharName, p?.MapName, (byte)m.Role, status));
                }

                client.EnqueuePacket(new IZ_GROUP_LIST
                {
                    AID = packet.AID,
                    ExpOption = g.GroupInfo.ExpOption,
                    GroupName = g.GroupInfo.GroupName,
                    CharinfoInGroup = charinfo.ToArray()
                });

                client.EnqueuePacket(new ZI_GRID_UPDATE
                {
                    AID = packet.AID,
                    ExpOption = g.GroupInfo.ExpOption,
                    GRID = grid.Value
                });
            }

            //var friends = _friendManager.GetFriends(packet.GID);
            return true;
        }

        private bool OnExit(ZoneClient client, ZI_EXIT packet)
        {
            Logger.Debug($"ZSRV({client.Sid}) ZI_EXIT Charactername={packet.CharacterName}");
            return ExitPlayer(client, packet.GID, packet.AID);
        }

        private bool ExitPlayer(ZoneClient client, int gid, int aid)
        {
            var player = _playerManager.FindPlayerByGID(gid);
            if (player == null)
                return true;

            player.Status = 0;
            var gdid = _guildManager.GetGDIDByGID(gid);
            if (gdid.HasValue)
            {
                var guild = _guildManager.GetGuild(gdid.Value);
                if (guild != null)
                {
                    var member = guild.GuildMInfo.FirstOrDefault(x => x.GID == player.GID);
                    if (member != null)
                    {
                        member.Status = player.Status;
                        _guildManager.UpdateGuildMember(gdid.Value, member.GID, member.Service, member.MemberExp, member.Level, member.Job);
                        BroadcastPacket(new IZ_UPDATE_CHARSTAT
                        {
                            GDID = gdid.Value,
                            GID = gid,
                            AID = aid,
                            Status = member.Status,
                            Sex = member.Sex,
                            Head = member.Head,
                            HeadPalette = member.HeadPalette,
                            Job = (short)member.Job,
                            Level = member.Level
                        });
                    }

                    // get other online players on this zone, if none, free guild
                    var onlineMembers = guild.GuildMInfo.Where(x => x.Status == 1 && _playerManager.FindPlayerByGID(x.GID)?.ZSID == client.Sid);
                    if (!onlineMembers.Any())
                    {
                        client.EnqueuePacket(new IZ_FREE_GUILD
                        {
                            GDID = player.GDID
                        });

                        _guildManager.FreeGuild(player.GDID);
                    }
                }
            }

            var grid = _groupManager.GetMember(gid);
            if (grid.HasValue)
            {
                var g = _groupManager.GetGroup(grid.Value);
                var offlineMember = g.GroupMember.FirstOrDefault(x => x.GID == gid);
                foreach (var m in g.GroupMember.Where(x => x.GID != gid))
                {
                    ZonePacket(player.ZSID, new IZ_ADD_MEMBER_TO_GROUP2
                    {
                        ReceiverAID = m.AID,
                        AID = player.AID,
                        Role = offlineMember?.Role ?? 0,
                        State = 1,
                        GroupName = g.GroupInfo.GroupName,
                        CharacterName = player.CharacterName,
                        MapName = player.MapName,
                        ItemPickupRule = g.GroupInfo.ItemPickupRule,
                        ItemDivisionRule = g.GroupInfo.ItemDivisionRule
                    });
                }
            }

            _playerManager.FreePlayer(aid);
            return true;

        }

        private bool OnReqUserCount(ZoneClient client, ZI_REQ_USER_COUNT packet)
        {
            Logger.Debug($"ZSRV({client.Sid}) ZI_REQ_USER_COUNT AID={packet.AID}");
            return true;
        }

        private bool OnReqUserClanInfo(ZoneClient client, ZI_REQ_USER_CLANINFO packet)
        {
            Logger.Debug($"ZSRV({client.Sid}) ZI_REQ_USER_CLANINFO GID={packet.GID}");
            return true;
        }

        private bool OnMakeGroup2(ZoneClient client, ZI_MAKE_GROUP2 packet)
        {
            Logger.Debug($"ZSRV({client.Sid}) ZI_MAKE_GROUP2 GroupName={packet.GroupName}");
            client.EnqueuePacket(new IZ_ACK_MAKE_GROUP { AID = packet.AID, GRID = 1, GroupName = packet.GroupName, Result = 0 });
            return true;
        }

        private bool OnReqLeaveGroup(ZoneClient client, ZI_REQ_LEAVE_GROUP packet)
        {
            Logger.Debug($"ZSRV({client.Sid}) ZI_REQ_LEAVE_GROUP GID={packet.GID}, AID={packet.AID}");
            return true;
        }

        private bool OnReqJoinClan(ZoneClient client, ZI_REQ_JOIN_CLAN packet)
        {
            Logger.Debug($"ZSRV({client.Sid}) ZI_REQ_JOIN_CLAN ClanID={packet.ClanID}");
            return true;
        }

        private bool OnReqMakeGuild(ZoneClient client, ZI_REQ_MAKE_GUILD packet)
        {
            Logger.Debug($"ZSRV({client.Sid}) ZI_REQ_MAKE_GUILD GName={packet.GName}, MName={packet.MName}, AccountName={packet.AccountName}");
            return true;
        }

        private bool OnReqBanGuild(ZoneClient client, ZI_REQ_BAN_GUILD packet)
        {
            Logger.Debug($"ZSRV({client.Sid}) ZI_REQ_BAN_GUILD GDID={packet.GDID}, AID={packet.AID}, Reason={packet.ReasonDesc}");
            return true;
        }

        private bool OnWhisper(ZoneClient client, ZI_WHISPER packet)
        {
            Logger.Debug($"ZSRV({client.Sid}) ZI_WHISPER SenderAID={packet.SenderAID}, Sender={packet.Sender}, Receiver={packet.Receiver}, SenderAccountName={packet.SenderAccountName}, Text={packet.Text}");
            var receiver = _playerManager.FindPlayerByName(packet.Receiver);
            if (receiver == null)
            {
                client.EnqueuePacket(new IZ_ACK_WHISPER { SenderAID = packet.SenderAID, Result = 1 });
                return true;
            }

            ZonePacket(receiver.ZSID, new IZ_WHISPER { ReceiverAID = receiver.AID, Sender = packet.Sender, SenderAccountName = packet.SenderAccountName, SenderAID = packet.SenderAID, Text = packet.Text });
            return true;
        }

        private bool OnAckWhisper(ZoneClient client, ZI_ACK_WHISPER packet)
        {
            Logger.Debug($"ZSRV({client.Sid}) ZI_ACK_WHISPER SenderAID={packet.SenderAID}, Sender={packet.Sender}, Sender={packet.Sender}, Result={packet.Result}");
            var receiver = _playerManager.FindPlayerByName(packet.Sender);
            ZonePacket(receiver.ZSID, new IZ_ACK_WHISPER { SenderAID = packet.SenderAID, Result = packet.Result });
            return true;
        }

        private bool OnBroadcast(ZoneClient client, ZI_BROADCAST packet)
        {
            Logger.Debug($"ZSRV({client.Sid}) ZI_BROADCAST Text={packet.Text}");
            BroadcastPacket(new IZ_BROADCAST { Text = packet.Text });
            return true;
        }

        private bool OnGuildChat(ZoneClient client, ZI_GUILD_CHAT packet)
        {
            Logger.Debug($"ZSRV({client.Sid}) ZI_GUILD_CHAT Text={packet.Text}");
            BroadcastPacket(new IZ_GUILD_CHAT { GDID = packet.GDID, Text = packet.Text });
            return true;
        }

        private bool OnMoveServer(ZoneClient client, ZI_MOVE_SERVER packet)
        {
            Logger.Debug($"ZSRV({client.Sid}) ZI_MOVE_SERVER CharName={packet.CharName}");
            return true;
        }

        private bool OnMapMove(ZoneClient client, ZI_MAPMOVE packet)
        {
            Logger.Debug($"ZSRV({client.Sid}) ZI_MAPMOVE AID={packet.AID} MapName={packet.MapName}");
            return true;
        }

        private bool OnGuildNotice(ZoneClient client, ZI_GUILD_NOTICE packet)
        {
            Logger.Debug($"ZSRV({client.Sid}) ZI_GUILD_NOTICE GDID={packet.GDID}, Notice={packet.Notice}, Subject={packet.Subject}");
            return true;
        }

        private bool OnGDSkillUpdate(ZoneClient client, ZI_GDSKILL_UPDATE packet)
        {
            Logger.Debug($"ZSRV({client.Sid}) ZI_GDSKILL_UPDATE GDID={packet.GDID}");
            return true;
        }

        private bool OnInstantMapAllow(ZoneClient client, ZI_INSTANTMAP_ALLOW packet)
        {
            Logger.Debug($"ZSRV({client.Sid}) ZI_INSTANTMAP_ALLOW Allow={packet.Allow} Why={packet.Why}");
            return true;
        }

        private bool OnMemorialDungeonSubscription2(ZoneClient client, ZI_MEMORIALDUNGEON_SUBSCRIPTION2 packet)
        {
            Logger.Debug($"ZSRV({client.Sid}) ZI_MEMORIALDUNGEON_SUBSCRIPTION2 AID={packet.AID}, GID={packet.GID}, NickName={packet.NickName}, DungeonName={packet.DungeonName}");

            var zone = GetLowestResourceZone();
            var memorialDungeon = _memorialDungeonManager.Subscribe(packet.AID, packet.GID, packet.GRID, packet.NickName, packet.DungeonName, zone.Sid);
            var createMap = memorialDungeon.Maps.FirstOrDefault(x => !x.Created);
            if (createMap == null)
            {
                Logger.Error($"Memorial Dungeon has no maps {packet.DungeonName}");
                return true;
            }

            ZonePacket(zone.Sid, new IZ_INSTANTMAP_CREATE_REQ
            {
                MapId = createMap.MapId,
                MapName = createMap.MapName,
                MapType = createMap.MapType,
                RequestN2Obj = createMap.RequestN2Obj
            });

            var group = _groupManager.GetGroup(packet.GRID);
            // send to zones with player
            foreach (var member in group.GroupMember)
            {
                var player = _playerManager.FindPlayerByGID(member.GID);
                if (player == null || player.Status == 0)
                {
                    continue;
                }

                ZonePacket(player.ZSID, new IZ_MEMORIALDUNGEON_SUBSCRIPTION_INFO2
                {
                    AID = player.AID,
                    GID = player.GID,
                    DungeonName = packet.DungeonName,
                    PriorityOrderNum = 1
                });
            }
            return true;
        }

        private bool OnInstantMapCreateRes(ZoneClient client, ZI_INSTANTMAP_CREATE_RES packet)
        {
            Logger.Debug($"ZSRV({client.Sid}) ZI_INSTANTMAP_CREATE_RES MapId={packet.MapId}, MapName={packet.MapName}, RequestN2Obj={packet.RequestN2Obj}, Success={packet.Success}");
            if (!packet.Success)
            {
                return true;
            }

            BroadcastPacket(new IZ_INSTANTMAP_ADD3
            {
                MapId = packet.MapId,
                MapName = packet.MapName,
                PlayerEnter = true,
                MapType = 20,
                ZSID = client.Sid
            });

            BroadcastPacket(new IZ_INSTANTMAP_PLAYER_ENTER3
            {
                MapId = packet.MapId,
                PlayerEnter = true
            });

            var memorialDungeon = _memorialDungeonManager.CreateResult(packet.MapId, packet.MapName, packet.RequestN2Obj, packet.Success);
            var createMap = memorialDungeon.Maps.FirstOrDefault(x => !x.Created);
            if (createMap != null)
            {
                ZonePacket(memorialDungeon.ZsId, new IZ_INSTANTMAP_CREATE_REQ
                {
                    MapId = createMap.MapId,
                    MapName = createMap.MapName,
                    MapType = createMap.MapType,
                    RequestN2Obj = createMap.RequestN2Obj
                });
                return true;
            }

            var group = _groupManager.GetGroup(memorialDungeon.GRID);
            BroadcastPacket(new IZ_MEMORIALDUNGEON_SYNC2
            {
                PartyID = memorialDungeon.GRID,
                GroupName = group.GroupInfo.GroupName,
                ExistZSID = memorialDungeon.ZsId,
                MemorialDungeonID = 5,
                Factor = 1,
                Event = IZ_MEMORIALDUNGEON_SYNC2.EnumEVENT.EVENT_CREATE,
                DungeonName = memorialDungeon.DungeonName
            });

            // send to zones with player
            foreach (var member in group.GroupMember)
            {
                var player = _playerManager.FindPlayerByGID(member.GID);
                if (player == null || player.Status == 0)
                {
                    continue;
                }

                ZonePacket(player.ZSID, new IZ_MEMORIALDUNGEON_INFO2
                {
                    AID = player.AID,
                    GID = player.GID,
                    DungeonName = memorialDungeon.DungeonName,
                    DestroyDate = Convert.ToInt32(DateTime.Now.AddMinutes(360).Timestamp() / 1000),
                    EnterTimeOutDate = Convert.ToInt32(DateTime.Now.AddMinutes(3).Timestamp() / 1000)
                });
            }

            return true;
        }
    }
}
