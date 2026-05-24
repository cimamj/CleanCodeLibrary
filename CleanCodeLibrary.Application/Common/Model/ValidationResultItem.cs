using CleanCodeLibrary.Domain.Common.Validation;

namespace CleanCodeLibrary.Application.Common.Model
{
    public class ValidationResultItem
    {
        public string Code
        {
            get; set;
        }

        public string Message
        {
            get; set;
        }

        public ValidationType ValidationType
        {
            get; init;
        }

        public ValidationSeverity ValidationSeverity
        {
            get; init;
        }

        public static ValidationResultItem FromValidationItem(ValidationItem item)
        {
            return new ValidationResultItem
            {
                Code = item.Code,
                Message = item.Message,
                ValidationType = item.ValidationType,
                ValidationSeverity = item.ValidationSeverity,
            };
        }
    }
}
