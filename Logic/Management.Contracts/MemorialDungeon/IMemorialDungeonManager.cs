namespace Aegis.Logic.Management.Contracts.MemorialDungeon
{
    public interface IMemorialDungeonManager
    {
        Classes.MemorialDungeon Subscribe(int aid, int gid, int groupId, string nickName, string dungeonName, int zsid);
        Classes.MemorialDungeon CreateResult(int mapId, string mapName, int requestN2Obj, bool success);
    }
}