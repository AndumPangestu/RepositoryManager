using RepositoryManager.Core.Interfaces;

namespace RepositoryManager.Validators
{
    /// <summary>
    /// Validator for JSON content
    /// </summary>
    public class JsonContentValidator : IContentValidator
    {
        public bool Validate(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return false;

            var trimmed = content.Trim();
            return (trimmed.StartsWith("{") && trimmed.EndsWith("}")) ||
                   (trimmed.StartsWith("[") && trimmed.EndsWith("]"));
        }
    }
}