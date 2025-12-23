using RepositoryManager.Core.Interfaces;

namespace RepositoryManager.Validators
{
    /// <summary>
    /// Validator for XML content
    /// </summary>
    public class XmlContentValidator : IContentValidator
    {
        public bool Validate(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return false;

            var trimmed = content.Trim();
            return trimmed.StartsWith("<") && trimmed.EndsWith(">");
        }
    }
}