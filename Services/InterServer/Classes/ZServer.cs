using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Aegis.CrossCutting.GlobalDataClasses;
using Aegis.CrossCutting.Network;
using Aegis.CrossCutting.Network.Classes;
using Aegis.CrossCutting.Network.Packets;
using Aegis.CrossCutting.Network.Packets.IZ;
using Aegis.CrossCutting.Network.Packets.ZI;
using Aegis.Logic.ChatManagement.Contracts;
using Aegis.Logic.ClanManagement.Contracts;
using Aegis.Logic.FriendManagement.Contracts;
using Aegis.Logic.GroupManagement.Contracts;
using Aegis.Logic.GuildManagement.Contracts;
using Aegis.Logic.MemorialDungeonManagement.Contracts;
using Aegis.Logic.PlayerManagement.Contracts;
using Aegis.Logic.PlayerManagement.Contracts.Classes;
using Aegis.Logic.SiegeManagement.Contracts;
using log4net;

namespace Aegis.Services.InterServer.Classes
{
    public class ZServer
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public int SId { get; set; }
        public Socket Socket { private get; set; }
        public int NumTotalNPC { get; set; }
        public int UsedKBytesMemory { get; set; }

        private string _ip;
        private CancellationToken _ct;
        private readonly CancellationTokenSource _ts;
        private readonly ManualResetEvent _threadAbort;
        private readonly PacketBuffer _packetBuffer;
        private BlockingCollection<PacketBase> _packetQueue;

        private IChatManager _chatManager;
        private IClanManager _clanManager;
        private IFriendManager _friendManager;
        private IGroupManager _groupManager;
        private IGuildManager _guildManager;
        private IMemorialDungeonManager _memorialDungeonManager;
        private IPlayerManager _playerManager;
        private ISiegeManager _siegeManager;

        public ZServer(IChatManager chatManager, IClanManager clanManager, IFriendManager friendManager,
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

            _ts = new CancellationTokenSource();
            _ct = _ts.Token;
            _threadAbort = new ManualResetEvent(false);
            _packetBuffer = new PacketBuffer(_ct);
        }

        public void Start()
        {
            try
            {
                _ip = ((IPEndPoint)Socket.RemoteEndPoint).Address.ToString();
                Logger.Debug($"ZSRV({_ip}) accept");
                _packetQueue = new BlockingCollection<PacketBase>();
                var receive = Task.Factory.StartNew(Receive, _ts.Token);
                var send = Task.Factory.StartNew(Send, _ts.Token);
                var parse = Task.Factory.StartNew(ParsePackets, _ts.Token);

                var tasks = new List<Task> { receive, send, parse };
                var waitHandles = new WaitHandle[] { InterServer.Instance.ServerShutdown, _threadAbort };

                WaitHandle.WaitAny(waitHandles);
                _ts.Cancel();
                Task.WaitAll(tasks.ToArray());
                receive.Dispose();
                send.Dispose();
                _threadAbort.Dispose();
                Logger.Debug($"ZSRV({SId}) disconnected... ({_ip})");
            }
            catch (Exception ex)
            {
                Logger.Warn(ex.Message);
            }
        }

        private void Receive()
        {
            try
            {
                var byteRecv = new Byte[32769];
                do
                {
                    while (Socket.Available == 0)
                    {
                        if (_ct.IsCancellationRequested)
                        {
                            return;
                        }

                        var poll = Socket.Poll(1000, SelectMode.SelectRead);
                        var avail = Socket.Available == 0;
                        if ((poll && avail))
                        {
                            return;
                        }
                    }

                    var recvBytes = Socket.Receive(byteRecv, SocketFlags.None);
                    if (recvBytes == 0)
                    {
                        break;
                    }

                    Received(byteRecv, recvBytes);
                } while (true);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
            }
            finally
            {
                Logger.Debug("Receive Thread Canceled");
                _threadAbort.Set();
            }
        }

        protected virtual void Received(byte[] data, int length)
        {
            var appendData = new byte[] { };
            Array.Resize(ref appendData, length);
            Array.Copy(data, 0, appendData, 0, length);
            _packetBuffer.Append(appendData);
        }

        private void Send()
        {
            try
            {
                foreach (var p in _packetQueue.GetConsumingEnumerable(_ct))
                {
                    using (var ms = new MemoryStream())
                    {
                        using (var bw = new BinaryWriter(ms))
                        {
                            p.WriteTo(bw);
                            var length = PacketLengthManager.GetPacketInformation((PACKET_COMMAND)p.Command);
                            if (p.GetType().IsSubclassOf(typeof(PacketVarSize)))
                            {
                                ((PacketVarSize)p).SetLength(bw);
                            }

                            var send = ms.ToArray();
                            if (!IsPacketLengthSane(p, ms))
                            {
                                if (length != null)
                                {
                                    Logger.ErrorFormat("Client::Send() {0} (0x{1:X4}) wrong length {2} != {3}", ((PACKET_COMMAND)p.Command), p.Command, ms.Position, length);
                                }
                                else
                                {
                                    Logger.ErrorFormat("Client::Send() {0} (0x{1:X4}) wrong length {2}", ((PACKET_COMMAND)p.Command), p.Command, ms.Position);
                                }
                                return;
                            }

                            //Logger.Debug($"> ZSRV({SId}) {(PACKET_COMMAND)p.Command}");
                            //Logger.Debug(send.Hexdump());
                            Socket.Send(send);
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
            finally
            {
                Logger.Debug("Send Thread Canceled");
                _threadAbort.Set();
            }
        }

        private void ParsePackets()
        {
            try
            {
                while (Socket.Connected)
                {
                    try
                    {
                        var packet = _packetBuffer.Dequeue();
                        if (!OnPacket(packet))
                        {
                            return;
                        }
                    }
                    catch (BufferException uce)
                    {
                        Logger.ErrorFormat("{0} {1}\n{2}", uce.Message, uce.Pos, uce.Packet.Hexdump());
                    }
                    catch (OperationCanceledException)
                    {
                        // thread abort...
                        return;
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex.Message, ex);
                    }
                }
            }
            finally
            {
                Logger.Debug("Parse Thread Canceled");
                _threadAbort.Set();
            }
        }

        public void EnqueuePacket(PacketBase packet)
        {
            _packetQueue.Add(packet, _ct);
        }

        private bool IsPacketLengthSane(PacketBase packet, MemoryStream ms)
        {
            var length = PacketLengthManager.GetPacketInformation((PACKET_COMMAND)packet.Command);

            // Packets must have type (ushort) and sequence number (uint)
            if (ms.Length < 2)
            {
                return false;
            }

            if (length.HasValue && ms.Length != length.Value)
            {
                return false;
            }

            if (!length.HasValue)
            {
                var lenPos = 0;
                if (packet.GetType().IsSubclassOf(typeof(PacketVarSize)))
                {
                    lenPos = 2;
                }

                // Variable length packets must additioanlly have a length attribute (ushort)
                if (ms.Length < lenPos + 2)
                {
                    return false;
                }

                var offset = ms.Position;
                ms.Seek(lenPos, SeekOrigin.Begin);
                var size = ms.ReadByte() | (ms.ReadByte() << 8);
                ms.Seek(offset, SeekOrigin.Begin);

                if (size != ms.Length)
                {
                    return false;
                }
            }

            return true;
        }

        private bool OnPacket(PacketBase packet)
        {
            switch (packet.Command)
            {
                case (ushort)PACKET_COMMAND.ZI_AUTH_REQ:
                    return OnAuthRequest(packet as ZI_AUTH_REQ);

                case (ushort)PACKET_COMMAND.ZI_VERSION:
                    return OnVersion(packet as ZI_VERSION);

                case (ushort)PACKET_COMMAND.ZI_PING_LIVE:
                    return OnPingLive(packet as ZI_PING_LIVE);

                case (ushort)PACKET_COMMAND.ZI_STATEINFO:
                    return OnStateInfo(packet as ZI_STATEINFO);

                case (ushort)PACKET_COMMAND.ZI_LOGON:
                    return OnLogin(packet as ZI_LOGON);

                case (ushort)PACKET_COMMAND.ZI_EXIT:
                    return OnExit(packet as ZI_EXIT);

                case (ushort)PACKET_COMMAND.ZI_MAKE_GROUP2:
                    return OnMakeGroup2(packet as ZI_MAKE_GROUP2);

                case (ushort)PACKET_COMMAND.ZI_REQ_LEAVE_GROUP:
                    return OnReqLeaveGroup(packet as ZI_REQ_LEAVE_GROUP); 

                case (ushort)PACKET_COMMAND.ZI_REQ_MAKE_GUILD:
                    return OnReqMakeGuild(packet as ZI_REQ_MAKE_GUILD);

                case (ushort)PACKET_COMMAND.ZI_REQ_USER_CLANINFO:
                    return OnReqUserClanInfo(packet as ZI_REQ_USER_CLANINFO);

                case (ushort)PACKET_COMMAND.ZI_WHISPER:
                    return OnWhisper(packet as ZI_WHISPER);

                case (ushort)PACKET_COMMAND.ZI_ACK_WHISPER:
                    return OnAckWhisper(packet as ZI_ACK_WHISPER);

                case (ushort)PACKET_COMMAND.ZI_BROADCAST:
                    return OnBroadcast(packet as ZI_BROADCAST);

                case (ushort)PACKET_COMMAND.ZI_GUILD_CHAT:
                    return OnGuildChat(packet as ZI_GUILD_CHAT);

                case (ushort)PACKET_COMMAND.ZI_REQ_USER_COUNT:
                    return OnReqUserCount(packet as ZI_REQ_USER_COUNT);

                case (ushort)PACKET_COMMAND.ZI_MOVE_SERVER:
                    return OnMoveServer(packet as ZI_MOVE_SERVER);

                case (ushort)PACKET_COMMAND.ZI_MAPMOVE:
                    return OnMapMove(packet as ZI_MAPMOVE);

                case (ushort)PACKET_COMMAND.ZI_REQ_JOIN_CLAN:
                    return OnReqJoinClan(packet as ZI_REQ_JOIN_CLAN);

                case (ushort)PACKET_COMMAND.ZI_REQ_BAN_GUILD:
                    return OnReqBanGuild(packet as ZI_REQ_BAN_GUILD);

                case (ushort)PACKET_COMMAND.ZI_GUILD_NOTICE:
                    return OnGuildNotice(packet as ZI_GUILD_NOTICE);

                case (ushort)PACKET_COMMAND.ZI_GDSKILL_UPDATE:
                    return OnGDSkillUpdate(packet as ZI_GDSKILL_UPDATE);

                case (ushort)PACKET_COMMAND.ZI_INSTANTMAP_ALLOW:
                    return OnInstantMapAllow(packet as ZI_INSTANTMAP_ALLOW);

                case (ushort)PACKET_COMMAND.ZI_MEMORIALDUNGEON_SUBSCRIPTION2:
                    return OnMemorialDungeonSubscription2(packet as ZI_MEMORIALDUNGEON_SUBSCRIPTION2);

                case (ushort)PACKET_COMMAND.ZI_INSTANTMAP_CREATE_RES:
                    return OnInstantMapCreateRes(packet as ZI_INSTANTMAP_CREATE_RES);
            }

            return true;
        }

        private bool OnAuthRequest(ZI_AUTH_REQ packet)
        {
            Logger.Debug($"ZSRV({_ip}) auth success");
            SId = packet.ZSID;
            EnqueuePacket(new IZ_AUTH_ACK());
            EnqueuePacket(new IZ_REQ_EDIT_EXP { MonitorNum = 0, Death = 0, Drop = 0, Exp = 0 });
            Logger.Debug($"ZSRV({SId}) auth... complete");
            return true;
        }

        private bool OnVersion(ZI_VERSION packet)
        {
            Logger.Debug($"ZSRV({SId}) ZI_VERSION Version={packet.Version}");
            return true;
        }

        private bool OnPingLive(ZI_PING_LIVE packet)
        {
            EnqueuePacket(new IZ_PING_LIVE());
            return true;
        }

        private bool OnStateInfo(ZI_STATEINFO packet)
        {
            Logger.Debug($"ZSRV({SId}) ZI_STATEINFO Memory={packet.UsedKBytesMemory}, NPCs={packet.NumTotalNPC}");
            UsedKBytesMemory = packet.UsedKBytesMemory;
            NumTotalNPC = packet.NumTotalNPC;
            return true;
        }

        private bool OnLogin(ZI_LOGON packet)
        {
            Logger.Debug($"ZSRV({SId}) ZI_LOGON AccountName {packet.AccountName}, CharacterName {packet.CharacterName}, MapName {packet.MapName}");
            var myPlayers = _playerManager.FindPlayersByAID(packet.AID);
            if (myPlayers.Any())
            {
                EnqueuePacket(new IZ_DISCONNECT_CHARACTER { AID = packet.AID });
                return ExitPlayer(packet.GID, packet.AID);
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
                ZSID = SId,
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
                EnqueuePacket(new IZ_GUILDINFO_TOD
                {
                    AID = 0,
                    GDID = gdid.Value,
                    Data = new GUILDINFO(gi.GDID, gi.Level, gi.Name, gi.MName, gi.MaxUserNum, gi.UserNum, gi.Honor, gi.Virtue,
                        gi.Type, gi.Class, gi.Money, gi.ArenaWin, gi.ArenaLose, gi.ArenaDrawn, gi.ManageLand,
                        gi.Exp, gi.EmblemVersion, gi.Point, gi.Desc)
                });

                // GuildNotice
                EnqueuePacket(new IZ_GUILD_NOTICE
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

                EnqueuePacket(new IZ_GUILD_MEMBERINFO_TOD
                {
                    AID = 0,
                    GDID = gdid.Value,
                    GuildMInfo = gmi.Select(x => new GUILDMINFO(x.GID, x.CharName, x.AccountName, x.Level, x.Memo, x.Service,
                        x.MemberExp, x.GDID, x.AID, x.PositionID, x.Head, x.HeadPalette, x.Sex, x.Job, x.Status)).ToArray()
                });

                // GuildAllyInfo
                EnqueuePacket(new IZ_GUILD_ALLYINFO_TOD
                {
                    AID = 0,
                    GDID = gdid.Value,
                    GuildAllyInfo = g.GuildAllyInfo.Select(x => new GUILDALLYINFO(x.GDID, x.OpponentGDID, x.GuildName, x.Relation)).ToArray()
                });

                // GuildBanishInfo
                EnqueuePacket(new IZ_GUILD_BANISHINFO_TOD
                {
                    AID = 0,
                    GDID = gdid.Value,
                    GuildBanishInfo = g.GuildBanishInfo.Select(x => new GUILDBANISHINFO(x.GDID, x.MemberName, string.Empty, x.Reason, x.GID, x.AID)).ToArray()
                });

                // GuildMPosition
                EnqueuePacket(new IZ_GUILD_MPOSITION_TOD
                {
                    AID = 0,
                    GDID = gdid.Value,
                    GuildMPosition = g.GuildMPosition.Select(x => new GUILDMPOSITION(x.GDID, x.Grade, x.PosName, x.JoinRight, x.PenaltyRight, x.PositionID, x.Service)).ToArray()
                });

                // GuildSkill
                EnqueuePacket(new IZ_GUILD_NOTIFYSKILLDATA
                {
                    IsForceSend = 1,
                    SkillPoint = g.GuildSkill.Point,
                    GuildSkill = g.GuildSkill.Skills.Select(x => new GUILDSKILL(x.SkillId, x.Level)).ToArray(),
                    GDID = gdid.Value,
                });

                InterServer.Instance.BroadcastPacket(new IZ_UPDATE_CHARSTAT
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

                EnqueuePacket(new IZ_UPDATE_CHARGDID
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

            EnqueuePacket(new IZ_ACK_LOGON { AID = packet.AID, GID = packet.GID, Type = 0 });

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
                        InterServer.Instance.ZonePacket(p.ZSID, new IZ_ADD_MEMBER_TO_GROUP2
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

                        EnqueuePacket(new IZ_ADD_MEMBER_TO_GROUP2
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

                EnqueuePacket(new IZ_GROUP_LIST
                {
                    AID = packet.AID,
                    ExpOption = g.GroupInfo.ExpOption,
                    GroupName = g.GroupInfo.GroupName,
                    CharinfoInGroup = charinfo.ToArray()
                });

                EnqueuePacket(new ZI_GRID_UPDATE
                {
                    AID = packet.AID,
                    ExpOption = g.GroupInfo.ExpOption,
                    GRID = grid.Value
                });
            }

            //var friends = _friendManager.GetFriends(packet.GID);
            return true;
        }

        private bool OnExit(ZI_EXIT packet)
        {
            Logger.Debug($"ZSRV({SId}) ZI_EXIT Charactername={packet.CharacterName}");
            return ExitPlayer(packet.GID, packet.AID);
        }

        private bool ExitPlayer(int gid, int aid)
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
                        InterServer.Instance.BroadcastPacket(new IZ_UPDATE_CHARSTAT
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
                    var onlineMembers = guild.GuildMInfo.Where(x => x.Status == 1 && _playerManager.FindPlayerByGID(x.GID)?.ZSID == SId);
                    if (!onlineMembers.Any())
                    {
                        EnqueuePacket(new IZ_FREE_GUILD
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
                    InterServer.Instance.ZonePacket(player.ZSID, new IZ_ADD_MEMBER_TO_GROUP2
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

        private bool OnReqUserCount(ZI_REQ_USER_COUNT packet)
        {
            Logger.Debug($"ZSRV({SId}) ZI_REQ_USER_COUNT AID={packet.AID}");
            return true;
        }

        private bool OnReqUserClanInfo(ZI_REQ_USER_CLANINFO packet)
        {
            Logger.Debug($"ZSRV({SId}) ZI_REQ_USER_CLANINFO GID={packet.GID}");
            return true;
        }

        private bool OnMakeGroup2(ZI_MAKE_GROUP2 packet)
        {
            Logger.Debug($"ZSRV({SId}) ZI_MAKE_GROUP2 GroupName={packet.GroupName}");
            EnqueuePacket(new IZ_ACK_MAKE_GROUP { AID = packet.AID, GRID = 1, GroupName = packet.GroupName, Result = 0 });
            return true;
        }

        private bool OnReqLeaveGroup(ZI_REQ_LEAVE_GROUP packet)
        {
            Logger.Debug($"ZSRV({SId}) ZI_REQ_LEAVE_GROUP GID={packet.GID}, AID={packet.AID}");
            return true;
        }

        private bool OnReqJoinClan(ZI_REQ_JOIN_CLAN packet)
        {
            Logger.Debug($"ZSRV({SId}) ZI_REQ_JOIN_CLAN ClanID={packet.ClanID}");
            return true;
        }

        private bool OnReqMakeGuild(ZI_REQ_MAKE_GUILD packet)
        {
            Logger.Debug($"ZSRV({SId}) ZI_REQ_MAKE_GUILD GName={packet.GName}, MName={packet.MName}, AccountName={packet.AccountName}");
            return true;
        }

        private bool OnReqBanGuild(ZI_REQ_BAN_GUILD packet)
        {
            Logger.Debug($"ZSRV({SId}) ZI_REQ_BAN_GUILD GDID={packet.GDID}, AID={packet.AID}, Reason={packet.ReasonDesc}");
            return true;
        }

        private bool OnWhisper(ZI_WHISPER packet)
        {
            Logger.Debug($"ZSRV({SId}) ZI_WHISPER SenderAID={packet.SenderAID}, Sender={packet.Sender}, Receiver={packet.Receiver}, SenderAccountName={packet.SenderAccountName}, Text={packet.Text}");
            var receiver = _playerManager.FindPlayerByName(packet.Receiver);
            if (receiver == null)
            {
                EnqueuePacket(new IZ_ACK_WHISPER { SenderAID = packet.SenderAID, Result = 1 });
                return true;
            }

            InterServer.Instance.ZonePacket(receiver.ZSID, new IZ_WHISPER { ReceiverAID = receiver.AID, Sender = packet.Sender, SenderAccountName = packet.SenderAccountName, SenderAID = packet.SenderAID, Text = packet.Text });
            return true;
        }

        private bool OnAckWhisper(ZI_ACK_WHISPER packet)
        {
            Logger.Debug($"ZSRV({SId}) ZI_ACK_WHISPER SenderAID={packet.SenderAID}, Sender={packet.Sender}, Sender={packet.Sender}, Result={packet.Result}");
            var receiver = _playerManager.FindPlayerByName(packet.Sender);
            InterServer.Instance.ZonePacket(receiver.ZSID, new IZ_ACK_WHISPER { SenderAID = packet.SenderAID, Result = packet.Result });
            return true;
        }

        private bool OnBroadcast(ZI_BROADCAST packet)
        {
            Logger.Debug($"ZSRV({SId}) ZI_BROADCAST Text={packet.Text}");
            InterServer.Instance.BroadcastPacket(new IZ_BROADCAST { Text = packet.Text });
            return true;
        }

        private bool OnGuildChat(ZI_GUILD_CHAT packet)
        {
            Logger.Debug($"ZSRV({SId}) ZI_GUILD_CHAT Text={packet.Text}");
            InterServer.Instance.BroadcastPacket(new IZ_GUILD_CHAT { GDID = packet.GDID, Text = packet.Text });
            return true;
        }

        private bool OnMoveServer(ZI_MOVE_SERVER packet)
        {
            Logger.Debug($"ZSRV({SId}) ZI_MOVE_SERVER CharName={packet.CharName}");
            return true;
        }

        private bool OnMapMove(ZI_MAPMOVE packet)
        {
            Logger.Debug($"ZSRV({SId}) ZI_MAPMOVE MapName={packet.MapName}");
            return true;
        }

        private bool OnGuildNotice(ZI_GUILD_NOTICE packet)
        {
            Logger.Debug($"ZSRV({SId}) ZI_GUILD_NOTICE GDID={packet.GDID}, Notice={packet.Notice}, Subject={packet.Subject}");
            return true;
        }

        private bool OnGDSkillUpdate(ZI_GDSKILL_UPDATE packet)
        {
            Logger.Debug($"ZSRV({SId}) ZI_GDSKILL_UPDATE GDID={packet.GDID}");
            return true;
        }

        private bool OnInstantMapAllow(ZI_INSTANTMAP_ALLOW packet)
        {
            Logger.Debug($"ZSRV({SId}) ZI_INSTANTMAP_ALLOW Why={packet.Why}, Allow={packet.Allow}");
            return true;
        }

        private bool OnMemorialDungeonSubscription2(ZI_MEMORIALDUNGEON_SUBSCRIPTION2 packet)
        {
            Logger.Debug($"ZSRV({SId}) ZI_MEMORIALDUNGEON_SUBSCRIPTION2 AID={packet.AID}, GID={packet.GID}, NickName={packet.NickName}, DungeonName={packet.DungeonName}");

            var zsid = InterServer.Instance.GetLowestResourceZone();
            var memorialDungeon = _memorialDungeonManager.Subscribe(packet.AID, packet.GID, packet.GRID, packet.NickName, packet.DungeonName, zsid);
            var createMap = memorialDungeon.Maps.FirstOrDefault(x => !x.Created);
            if (createMap == null)
            {
                Logger.Error($"Memorial Dungeon has no maps {packet.DungeonName}");
                return true;
            }

            InterServer.Instance.ZonePacket(zsid, new IZ_INSTANTMAP_CREATE_REQ
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

                InterServer.Instance.ZonePacket(player.ZSID, new IZ_MEMORIALDUNGEON_SUBSCRIPTION_INFO2
                {
                    AID = player.AID,
                    GID = player.GID,
                    DungeonName = packet.DungeonName,
                    PriorityOrderNum = 1
                });
            }
            return true;
        }

        private bool OnInstantMapCreateRes(ZI_INSTANTMAP_CREATE_RES packet)
        {
            Logger.Debug($"ZSRV({SId}) ZI_INSTANTMAP_CREATE_RES MapId={packet.MapId}, MapName={packet.MapName}, RequestN2Obj={packet.RequestN2Obj}, Success={packet.Success}");
            if (!packet.Success)
            {
                return true;
            }

            InterServer.Instance.BroadcastPacket(new IZ_INSTANTMAP_ADD3
            {
                MapId = packet.MapId,
                MapName = packet.MapName,
                PlayerEnter = true,
                MapType = 20,
                ZSID = SId
            });

            InterServer.Instance.BroadcastPacket(new IZ_INSTANTMAP_PLAYER_ENTER3
            {
                MapId = packet.MapId,
                PlayerEnter = true
            });

            var memorialDungeon = _memorialDungeonManager.CreateResult(packet.MapId, packet.MapName, packet.RequestN2Obj, packet.Success);
            var createMap = memorialDungeon.Maps.FirstOrDefault(x => !x.Created);
            if (createMap != null)
            {
                InterServer.Instance.ZonePacket(memorialDungeon.ZsId, new IZ_INSTANTMAP_CREATE_REQ
                {
                    MapId = createMap.MapId,
                    MapName = createMap.MapName,
                    MapType = createMap.MapType,
                    RequestN2Obj = createMap.RequestN2Obj
                });
                return true;
            }

            var group = _groupManager.GetGroup(memorialDungeon.GRID);
            InterServer.Instance.BroadcastPacket(new IZ_MEMORIALDUNGEON_SYNC2
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

                InterServer.Instance.ZonePacket(player.ZSID, new IZ_MEMORIALDUNGEON_INFO2
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