
using CleanCodeLibrary.Domain.Common.Validation;

namespace CleanCodeLibrary.Application.Common.Model
{
    public class ValidationResultItem //samo mapiranje
    {
        public string Code { get; set; } //zasto ovo moze samo 1 stvar primiti a inace se ValidationResult iz domaina salje listu itema 
        public string Message { get; set; }
        public ValidationType ValidationType { get; init; }
        public ValidationSeverity ValidationSeverity { get; init; } //  kad init kad set

        public static ValidationResultItem FromValidationItem(ValidationItem item)  //prima iz domaina
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
