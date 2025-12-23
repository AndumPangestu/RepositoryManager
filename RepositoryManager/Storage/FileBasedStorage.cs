using System.Text.Json;
using RepositoryManager.Core.Interfaces;
using RepositoryManager.Core.Models;
using RepositoryManager.Core.Enums;
using RepositoryManager.Content;

namespace RepositoryManager.Storage
{
    /// <summary>
    /// File-based persistent storage implementation
    /// Each item is stored as a separate file in the specified directory
    /// </summary>
    public class FileBasedStorage : IRepositoryStorage
    {
        private readonly string _storageDirectory;
        private readonly object _fileLock = new object();

        public FileBasedStorage(string storageDirectory)
        {
            if (string.IsNullOrWhiteSpace(storageDirectory))
                throw new ArgumentException("Storage directory cannot be null or empty.", nameof(storageDirectory));

            _storageDirectory = storageDirectory;
        }

        public void Initialize()
        {
            if (!Directory.Exists(_storageDirectory))
            {
                Directory.CreateDirectory(_storageDirectory);
            }
        }

        public bool TryAdd(string key, RepositoryItem item)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Key cannot be null or empty.", nameof(key));

            if (item == null)
                throw new ArgumentNullException(nameof(item));

            lock (_fileLock)
            {
                var filePath = GetFilePath(key);

                if (File.Exists(filePath))
                    return false;

                try
                {
                    var fileData = new StoredItemData
                    {
                        Name = item.Name,
                        Content = item.Content.GetRawContent(),
                        Type = item.Content.Type
                    };

                    var json = JsonSerializer.Serialize(fileData, new JsonSerializerOptions
                    {
                        WriteIndented = true
                    });

                    File.WriteAllText(filePath, json);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool TryGet(string key, out RepositoryItem? item)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Key cannot be null or empty.", nameof(key));

            item = null;

            lock (_fileLock)
            {
                var filePath = GetFilePath(key);

                if (!File.Exists(filePath))
                    return false;

                try
                {
                    var json = File.ReadAllText(filePath);
                    var fileData = JsonSerializer.Deserialize<StoredItemData>(json);

                    if (fileData == null)
                        return false;

                    IContentData content = fileData.Type switch
                    {
                        ItemType.Json => new JsonContent(fileData.Content),
                        ItemType.Xml => new XmlContent(fileData.Content),
                        ItemType.Text => new TextContent(fileData.Content),
                        _ => throw new NotSupportedException($"Unsupported item type: {fileData.Type}")
                    };

                    item = new RepositoryItem(fileData.Name, content);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool TryRemove(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Key cannot be null or empty.", nameof(key));

            lock (_fileLock)
            {
                var filePath = GetFilePath(key);

                if (!File.Exists(filePath))
                    return false;

                try
                {
                    File.Delete(filePath);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool ContainsKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Key cannot be null or empty.", nameof(key));

            var filePath = GetFilePath(key);
            return File.Exists(filePath);
        }

        private string GetFilePath(string key)
        {
            // Sanitize key to be a valid filename
            var sanitizedKey = string.Join("_", key.Split(Path.GetInvalidFileNameChars()));
            return Path.Combine(_storageDirectory, $"{sanitizedKey}.json");
        }

        /// <summary>
        /// Internal class for serializing/deserializing repository items to files
        /// </summary>
        private class StoredItemData
        {
            public string Name { get; set; } = string.Empty;
            public string Content { get; set; } = string.Empty;
            public ItemType Type { get; set; }
        }
    }
}