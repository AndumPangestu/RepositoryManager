using RepositoryManager.Core.Enums;

namespace RepositoryManager.Core.Interfaces
{
    /// <summary>
    /// Represents strongly-typed content data that can be stored in the repository
    /// </summary>
    public interface IContentData
    {
        /// <summary>
        /// Gets the type of the content
        /// </summary>
        ItemType Type { get; }

        /// <summary>
        /// Gets the raw string representation of the content
        /// </summary>
        string GetRawContent();

        /// <summary>
        /// Validates the content format
        /// </summary>
        bool IsValid();
    }
}