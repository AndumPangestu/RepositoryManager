using RepositoryManager.Core.Enums;
using RepositoryManager.Core.Interfaces;

namespace RepositoryManager.Content
{
    /// <summary>
    /// Strongly-typed plain text content
    /// </summary>
    public class TextContent : IContentData
    {
        private readonly string _content;

        public ItemType Type => ItemType.Text;

        public TextContent(string content)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            _content = content;
        }

        public string GetRawContent() => _content;

        public bool IsValid()
        {
            // Text content is always valid
            return true;
        }
    }
}