using RepositoryManager.Core.Interfaces;
using RepositoryManager.Core.Models;
using RepositoryManager.Storage;

namespace RepositoryManager
{
    /// <summary>
    /// Main Repository Manager Class - Manages storage and retrieval of strongly-typed content
    /// </summary>
    public class RepositoryManager
    {
        private readonly IRepositoryStorage _storage;
        private bool _isInitialized;
        private readonly object _initLock = new object();

        /// <summary>
        /// Constructor with dependency injection for flexibility
        /// </summary>
        /// <param name="storage">Storage implementation (defaults to InMemoryStorage)</param>
        public RepositoryManager(IRepositoryStorage? storage = null)
        {
            _storage = storage ?? new InMemoryStorage();
            _isInitialized = false;
        }

        /// <summary>
        /// Initialize the repository for use - can only be called once
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if already initialized</exception>
        public void Initialize()
        {
            lock (_initLock)
            {
                if (_isInitialized)
                    throw new InvalidOperationException("Repository has already been initialized.");

                _storage.Initialize();
                _isInitialized = true;
            }
        }

        /// <summary>
        /// Store strongly-typed content to the repository
        /// </summary>
        /// <param name="itemName">Unique identifier for the item</param>
        /// <param name="content">Strongly-typed content to store</param>
        /// <exception cref="InvalidOperationException">Thrown if not initialized or item already exists</exception>
        /// <exception cref="ArgumentException">Thrown if parameters are invalid</exception>
        public void Register(string itemName, IContentData content)
        {
            EnsureInitialized();
            ValidateItemName(itemName);

            if (content == null)
                throw new ArgumentNullException(nameof(content));

            if (!content.IsValid())
                throw new ArgumentException($"Invalid {content.Type} content format.", nameof(content));

            // Protect from overwriting
            if (_storage.ContainsKey(itemName))
                throw new InvalidOperationException($"Item '{itemName}' already exists and cannot be overwritten.");

            var item = new RepositoryItem(itemName, content);

            if (!_storage.TryAdd(itemName, item))
                throw new InvalidOperationException($"Failed to register item '{itemName}'.");
        }

        /// <summary>
        /// Retrieve strongly-typed content from the repository
        /// </summary>
        /// <param name="itemName">Unique identifier of the item</param>
        /// <returns>Strongly-typed content data</returns>
        /// <exception cref="InvalidOperationException">Thrown if not initialized</exception>
        /// <exception cref="KeyNotFoundException">Thrown if item not found</exception>
        public IContentData Retrieve(string itemName)
        {
            EnsureInitialized();
            ValidateItemName(itemName);

            if (!_storage.TryGet(itemName, out var item) || item == null)
                throw new KeyNotFoundException($"Item '{itemName}' not found in repository.");

            return item.Content;
        }

        /// <summary>
        /// Remove an item from the repository
        /// </summary>
        /// <param name="itemName">Unique identifier of the item</param>
        /// <exception cref="InvalidOperationException">Thrown if not initialized</exception>
        /// <exception cref="KeyNotFoundException">Thrown if item not found</exception>
        public void Deregister(string itemName)
        {
            EnsureInitialized();
            ValidateItemName(itemName);

            if (!_storage.TryRemove(itemName))
                throw new KeyNotFoundException($"Item '{itemName}' not found in repository or could not be removed.");
        }

        /// <summary>
        /// Check if an item exists in the repository
        /// </summary>
        /// <param name="itemName">Unique identifier of the item</param>
        /// <returns>True if item exists, false otherwise</returns>
        public bool Contains(string itemName)
        {
            EnsureInitialized();

            if (string.IsNullOrWhiteSpace(itemName))
                return false;

            return _storage.ContainsKey(itemName);
        }

        // Private helper methods
        private void EnsureInitialized()
        {
            if (!_isInitialized)
                throw new InvalidOperationException("Repository must be initialized before use. Call Initialize() first.");
        }

        private void ValidateItemName(string itemName)
        {
            if (string.IsNullOrWhiteSpace(itemName))
                throw new ArgumentException("Item name cannot be null or empty.", nameof(itemName));
        }
    }
}