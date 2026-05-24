using CleanCodeLibrary.Domain.Common.Validation;

namespace CleanCodeLibrary.Application.Common.Model
{
    public class Result<TValue> where TValue : class
    {
        private List<ValidationResultItem> _info = new List<ValidationResultItem>();

        private List<ValidationResultItem> _errors = new List<ValidationResultItem>();

        private List<ValidationResultItem> _warnings = new List<ValidationResultItem>();

        public TValue? Value
        {
            get; set;
        }

        public Guid RequestId
        {
            get; set;
        }

        public bool IsAuthorized { get; set; } = true;

        public IReadOnlyList<ValidationResultItem> Errors => _errors.AsReadOnly();

        public IReadOnlyList<ValidationResultItem> Warnings => _warnings.AsReadOnly();

        public IReadOnlyList<ValidationResultItem> Info => _info.AsReadOnly();

        public bool HasError => _errors.Any();

        public bool HasWarning => _warnings.Any();

        public void AddError(ValidationResultItem item) => _errors.Add(item);

        public void AddWarning(ValidationResultItem item) => _warnings.Add(item);

        public void AddInfo(ValidationResultItem item) => _info.Add(item);

        public void SetResult(TValue value)
        {
            Value = value;
        }

        public void SetValidationResult(ValidationResult validationResult)
        {
            var errors = validationResult.ValidationItems
                .Where(x => x.ValidationSeverity == ValidationSeverity.Error)
                .Select(x => ValidationResultItem.FromValidationItem(x));

            _errors.AddRange(errors);

            var warnings = validationResult.ValidationItems
                .Where(x => x.ValidationSeverity == ValidationSeverity.Warning)
                .Select(x => ValidationResultItem.FromValidationItem(x));

            _warnings.AddRange(warnings);

            var info = validationResult.ValidationItems
               .Where(x => x.ValidationSeverity == ValidationSeverity.Info)
               .Select(x => ValidationResultItem.FromValidationItem(x));

            _info.AddRange(info);
        }

        public void SetUnauthorizedResult()
        {
            Value = null;
            IsAuthorized = false;
        }
    }
}
