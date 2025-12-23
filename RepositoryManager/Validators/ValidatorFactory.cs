using RepositoryManager.Core.Enums;
using RepositoryManager.Core.Interfaces;

namespace RepositoryManager.Validators
{
    /// <summary>
    /// Factory for creating content validators based on item type
    /// </summary>
    public class ValidatorFactory
    {
        private readonly Dictionary<ItemType, IContentValidator> _validators;

        public ValidatorFactory()
        {
            _validators = new Dictionary<ItemType, IContentValidator>
            {
                { ItemType.Json, new JsonContentValidator() },
                { ItemType.Xml, new XmlContentValidator() }
            };
        }

        /// <summary>
        /// Gets a validator for the specified item type
        /// </summary>
        public IContentValidator GetValidator(ItemType itemType)
        {
            if (_validators.TryGetValue(itemType, out var validator))
                return validator;

            throw new NotSupportedException($"Item type {itemType} is not supported.");
        }
    }
}