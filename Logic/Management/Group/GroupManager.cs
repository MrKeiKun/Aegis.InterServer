using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Aegis.Data.Repositories.Contracts;
using Aegis.Logic.Management.Contracts.Group;
using Aegis.Logic.Management.Contracts.Group.Classes;
using AutoMapper;

namespace Aegis.Logic.Management.Group
{
    public class GroupManager : IGroupManager
    {
        private readonly ConcurrentDictionary<int, Contracts.Group.Classes.Group> _groups;

        private readonly ICharacterRepository _characterRepository;

        public GroupManager(ICharacterRepository characterRepository)
        {
            _characterRepository = characterRepository;
            _groups = new ConcurrentDictionary<int, Contracts.Group.Classes.Group>();
        }

        public int? GetMember(int gid)
        {
            return _characterRepository.GetMember(gid);
        }

        public Contracts.Group.Classes.Group GetGroup(int groupId)
        {
            var group = _groups.FirstOrDefault(x => x.Key == groupId);
            if (group.Value == null)
            {
                var g = new Contracts.Group.Classes.Group
                {
                    GroupInfo = Mapper.Map<GroupInfo>(_characterRepository.GetGroupInfo(groupId)),
                    GroupMember = Mapper.Map<IEnumerable<GroupMember>>(_characterRepository.GetGroupMembers(groupId)),
                };

                _groups.TryAdd(groupId, g);
            }

            return _groups[groupId];
        }
    }
}