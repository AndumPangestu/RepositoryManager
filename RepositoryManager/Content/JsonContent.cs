using RepositoryManager.Core.Enums;
using RepositoryManager.Core.Interfaces;

namespace RepositoryManager.Content
{
    /// <summary>
    /// Strongly-typed JSON content
    /// </summary>
    public class JsonContent : IContentData
    {
        private readonly string _content;

        public ItemType Type => ItemType.Json;

        public JsonContent(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("JSON content cannot be null or empty.", nameof(content));

            _content = content;

            if (!IsValid())
                throw new ArgumentException("Invalid JSON format.", nameof(content));
        }

        public string GetRawContent() => _content;

        public bool IsValid()
        {
            var trimmed = _content.Trim();
            return (trimmed.StartsWith("{") && trimmed.EndsWith("}")) ||
                   (trimmed.StartsWith("[") && trimmed.EndsWith("]"));
        }
    }
}