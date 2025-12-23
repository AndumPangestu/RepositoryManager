namespace RepositoryManager.Core.Enums
{
    /// <summary>
    /// Defines the types of content that can be stored in the repository
    /// </summary>
    public enum ItemType
    {
        /// <summary>
        /// JSON formatted content
        /// </summary>
        Json = 1,

        /// <summary>
        /// XML formatted content
        /// </summary>
        Xml = 2,

        /// <summary>
        /// Plain text content
        /// </summary>
        Text = 3
    }
}