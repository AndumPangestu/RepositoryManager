using RepositoryManager.Core.Models;

namespace RepositoryManager.Core.Interfaces
{
    /// <summary>
    /// Interface for storage abstraction - supports multiple storage implementations
    /// </summary>
    public interface IRepositoryStorage
    {
        /// <summary>
        /// Attempts to add an item to storage
        /// </summary>
        bool TryAdd(string key, RepositoryItem item);

        /// <summary>
        /// Attempts to retrieve an item from storage
        /// </summary>
        bool TryGet(string key, out RepositoryItem? item);

        /// <summary>
        /// Attempts to remove an item from storage
        /// </summary>
        bool TryRemove(string key);

        /// <summary>
        /// Checks if a key exists in storage
        /// </summary>
        bool ContainsKey(string key);

        /// <summary>
        /// Initializes the storage (e.g., creates directories, connections)
        /// </summary>
        void Initialize();
    }
}