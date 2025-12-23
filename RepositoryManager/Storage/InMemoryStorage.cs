using System.Collections.Concurrent;
using RepositoryManager.Core.Interfaces;
using RepositoryManager.Core.Models;

namespace RepositoryManager.Storage
{
    /// <summary>
    /// Thread-safe in-memory storage implementation using ConcurrentDictionary
    /// </summary>
    public class InMemoryStorage : IRepositoryStorage
    {
        private readonly ConcurrentDictionary<string, RepositoryItem> _storage;

        public InMemoryStorage()
        {
            _storage = new ConcurrentDictionary<string, RepositoryItem>(StringComparer.OrdinalIgnoreCase);
        }

        public void Initialize()
        {
            // No initialization needed for in-memory storage
        }

        public bool TryAdd(string key, RepositoryItem item)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Key cannot be null or empty.", nameof(key));

            if (item == null)
                throw new ArgumentNullException(nameof(item));

            return _storage.TryAdd(key, item);
        }

        public bool TryGet(string key, out RepositoryItem? item)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Key cannot be null or empty.", nameof(key));

            return _storage.TryGetValue(key, out item);
        }

        public bool TryRemove(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Key cannot be null or empty.", nameof(key));

            return _storage.TryRemove(key, out _);
        }

        public bool ContainsKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Key cannot be null or empty.", nameof(key));

            return _storage.ContainsKey(key);
        }
    }
}