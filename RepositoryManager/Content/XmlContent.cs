using RepositoryManager.Core.Enums;
using RepositoryManager.Core.Interfaces;

namespace RepositoryManager.Content
{
    /// <summary>
    /// Strongly-typed XML content
    /// </summary>
    public class XmlContent : IContentData
    {
        private readonly string _content;

        public ItemType Type => ItemType.Xml;

        public XmlContent(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("XML content cannot be null or empty.", nameof(content));

            _content = content;

            if (!IsValid())
                throw new ArgumentException("Invalid XML format.", nameof(content));
        }

        public string GetRawContent() => _content;

        public bool IsValid()
        {
            var trimmed = _content.Trim();
            return trimmed.StartsWith("<") && trimmed.EndsWith(">");
        }
    }
}