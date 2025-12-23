namespace RepositoryManager.Core.Interfaces
{
    /// <summary>
    /// Contract for content validation
    /// </summary>
    public interface IContentValidator
    {
        /// <summary>
        /// Validates the content format
        /// </summary>
        /// <param name="content">Content to validate</param>
        /// <returns>True if valid, false otherwise</returns>
        bool Validate(string content);
    }
}