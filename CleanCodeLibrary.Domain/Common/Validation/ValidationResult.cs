namespace CleanCodeLibrary.Domain.Common.Validation
{
    public class ValidationResult
    {
        private List<ValidationItem> _validationItems = new List<ValidationItem>();

        public IReadOnlyList<ValidationItem> ValidationItems => _validationItems;

        public bool HasError => _validationItems.Any(validationItem => validationItem.ValidationSeverity == ValidationSeverity.Error);

        public bool HasInfo => _validationItems.Any(validationItem => validationItem.ValidationSeverity == ValidationSeverity.Info);

        public bool HasWarning => _validationItems.Any(validationItem => validationItem.ValidationSeverity == ValidationSeverity.Warning);

        public void AddValidationItem(ValidationItem validationItem)
        {
            _validationItems.Add(validationItem);
        }
    }
}
