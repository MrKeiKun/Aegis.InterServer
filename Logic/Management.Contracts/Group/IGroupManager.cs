namespace Aegis.Logic.Management.Contracts.Group
{
    public interface IGroupManager
    {
        /// <summary>
        /// Returns the GroupId for a player. May be null.
        /// </summary>
        /// <param name="gid"></param>
        /// <returns></returns>
        int? GetMember(int gid);
        
        Classes.Group GetGroup(int groupId);
    }
}