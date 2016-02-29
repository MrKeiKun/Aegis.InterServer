using System.Collections.Concurrent;

namespace Aegis.CrossCutting.GlobalDataClasses
{
    public class Cache<T>
    {
        private ConcurrentDictionary<int, T> _cache;

        public Cache()
        {
            _cache = new ConcurrentDictionary<int, T>();
        }

        public void Add(T entry, int key)
        {
            //_cache.
        }
    }
}