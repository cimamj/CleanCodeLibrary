namespace CleanCodeLibrary.Domain.Common.Validation
{
    public class ValidationItem
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
    }
}
