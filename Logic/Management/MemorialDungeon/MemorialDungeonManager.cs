using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Aegis.Logic.Management.Contracts.MemorialDungeon;
using Aegis.Logic.Management.Contracts.MemorialDungeon.Classes;
using Aegis.Logic.Management.Contracts.MemorialDungeon.Exceptions;

namespace Aegis.Logic.Management.MemorialDungeon
{
    public class MemorialDungeonManager : IMemorialDungeonManager
    {
        private readonly ConcurrentDictionary<int, Contracts.MemorialDungeon.Classes.MemorialDungeon> _dungeons;

        public MemorialDungeonManager()
        {
            _dungeons = new ConcurrentDictionary<int, Contracts.MemorialDungeon.Classes.MemorialDungeon>();
        }

        public Contracts.MemorialDungeon.Classes.MemorialDungeon Subscribe(int aid, int gid, int groupId, string nickName, string dungeonName, int zsid)
        {
            if (_dungeons.ContainsKey(groupId))
            {
                throw new SubscriptionErrorDuplicateException();
            }

            var md = new Contracts.MemorialDungeon.Classes.MemorialDungeon
            {
                AID = aid,
                GID = gid,
                GRID = groupId,
                DungeonName = dungeonName,
                ZsId = zsid,
                Maps = new List<MemorialDungeonMap>()
            };

            md.Maps.Add(new MemorialDungeonMap
            {
                MapId = 10000,
                MapName = "00q1@tower.gat",
                MapType = 20,
                RequestN2Obj = 1,
                Created = false
            });

            md.Maps.Add(new MemorialDungeonMap
            {
                MapId = 10001,
                MapName = "00q2@tower.gat",
                MapType = 20,
                RequestN2Obj = 2,
                Created = false
            });

            md.Maps.Add(new MemorialDungeonMap
            {
                MapId = 10002,
                MapName = "00q3@tower.gat",
                MapType = 20,
                RequestN2Obj = 3,
                Created = false
            });

            _dungeons.TryAdd(groupId, md);
            return md;
        }

        public Contracts.MemorialDungeon.Classes.MemorialDungeon CreateResult(int mapId, string mapName, int requestN2Obj, bool success)
        {
            var dungeon = _dungeons.FirstOrDefault(x => x.Value.Maps.Any(y => y.MapId == mapId)).Value;
            var memorialDungeonMap = dungeon.Maps.FirstOrDefault(x => x.MapId == mapId);
            if (memorialDungeonMap != null) { memorialDungeonMap.Created = success;}
            return dungeon;
        }
    }
}