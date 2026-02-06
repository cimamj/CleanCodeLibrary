using CleanCodeLibrary.Domain.Common.Validation;

namespace CleanCodeLibrary.Domain.Common.Model
{
    public class ResultDomain<TValue> 
    {
        public TValue Value { get; set; } //bool, int MORE LI I IENUMERABLE??
        public ValidationResult ValidationResult { get; private set; }

        public ResultDomain(TValue value, ValidationResult validationResult) 
        { 
            Value = value;
            ValidationResult = validationResult; 
        }
    }
}
