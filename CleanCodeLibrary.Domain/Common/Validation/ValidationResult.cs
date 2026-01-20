
namespace CleanCodeLibrary.Domain.Common.Validation
{
    public class ValidationResult
    {
        private List<ValidationItem> _validationItems = new List<ValidationItem>();  //svi enumi izmjesani, jer domain ne brine sto ce api klijent pokazati, app vec ima odvojeno cisce jednostavnije za slati na frontend
        public IReadOnlyList<ValidationItem> ValidationItems => _validationItems;
        public bool HasError => _validationItems.Any(validationItem => validationItem.ValidationSeverity == ValidationSeverity.Error);
        public bool HasInfo => _validationItems.Any(validationItem => validationItem.ValidationSeverity == ValidationSeverity.Info);
        public bool HasWarning => _validationItems.Any(validationItem => validationItem.ValidationSeverity == ValidationSeverity.Warning);
        //samo se ovo iz validacija salje, pa ako ocu vidit ima li errora, warninga odavde trebam vaditi

        //dodaj mu ovo polje validationItem
        public void AddValidationItem(ValidationItem validationItem) 
        { 
            _validationItems.Add(validationItem);
        }

    }
}
