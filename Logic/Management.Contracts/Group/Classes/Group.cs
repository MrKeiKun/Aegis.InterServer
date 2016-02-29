using System.Collections.Generic;

namespace Aegis.Logic.Management.Contracts.Group.Classes
{
    public class Group
    {
        public GroupInfo GroupInfo { get; set; }
        public IEnumerable<GroupMember> GroupMember { get; set; }
    }
}
