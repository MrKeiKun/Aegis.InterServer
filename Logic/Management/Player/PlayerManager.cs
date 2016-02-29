using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Aegis.CrossCutting.GlobalDataClasses;
using Aegis.Data.Repositories.Contracts;
using Aegis.Logic.Management.Contracts.Player;

namespace Aegis.Logic.Management.Player
{
    public class PlayerManager : IPlayerManager
    {
        private readonly ICharacterRepository _characterRepository;

        private readonly ConcurrentDictionary<int, Contracts.Player.Classes.Player> _playerCache;

        public PlayerManager(ICharacterRepository characterRepository)
        {
            _characterRepository = characterRepository;
            _playerCache = new ConcurrentDictionary<int, Contracts.Player.Classes.Player>();
        }

        public IEnumerable<Contracts.Player.Classes.Player> FindPlayersByAID(int aid)
        {
            return _playerCache.Where(x => x.Value.AID == aid).Select(x => x.Value).ToArray();
        }

        public Contracts.Player.Classes.Player FindPlayerByGID(int gid)
        {
            return _playerCache.FirstOrDefault(x => x.Key == gid).Value;
        }

        public Contracts.Player.Classes.Player FindPlayerByName(string name)
        {
            return _playerCache.FirstOrDefault(x => x.Value.CharacterName == name).Value;
        }

        public void AddPlayer(Contracts.Player.Classes.Player player)
        {
            _playerCache.TryAdd(player.GID, player);
        }

        public void FreePlayer(int aid)
        {
            var players = FindPlayersByAID(aid);
            foreach (var player in players)
            {
                Contracts.Player.Classes.Player p;
                _playerCache.TryRemove(player.GID, out p);
            }
        }

        public IEnumerable<MakerRank> GetTopMakerRank(int makerType)
        {
            return _characterRepository.GetTopMakerRank(makerType);
        }
    }
}