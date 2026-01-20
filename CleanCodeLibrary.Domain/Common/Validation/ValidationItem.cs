
namespace CleanCodeLibrary.Domain.Common.Validation
{
    public class ValidationItem
    {
        public string Code { get; set; } //koji entitet baca gresku
        public string Message { get; set; } //predugo ime ili predugo prezime...
        public ValidationType ValidationType { get; init; }
        public ValidationSeverity ValidationSeverity { get; init; }

    }
}
