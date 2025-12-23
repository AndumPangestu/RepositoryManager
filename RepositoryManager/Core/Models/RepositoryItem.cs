using RepositoryManager.Core.Interfaces;

namespace RepositoryManager.Core.Models
{
    /// <summary>
    /// Model for storing repository items with strongly-typed content
    /// </summary>
    public class RepositoryItem
    {
        /// <summary>
        /// Unique identifier for the item
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Strongly-typed content data
        /// </summary>
        public IContentData Content { get; }

        public RepositoryItem(string name, IContentData content)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty.", nameof(name));

            Name = name;
            Content = content ?? throw new ArgumentNullException(nameof(content));
        }
    }
}