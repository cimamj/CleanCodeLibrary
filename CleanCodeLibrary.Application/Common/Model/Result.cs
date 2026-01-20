

using CleanCodeLibrary.Domain.Common.Validation;

namespace CleanCodeLibrary.Application.Common.Model
{
    public class Result<TValue> where TValue : class //isto kao ValidationResult iz domaina
    {
        private List<ValidationResultItem> _info = new List<ValidationResultItem>();
        private List<ValidationResultItem> _errors = new List<ValidationResultItem>();
        private List<ValidationResultItem> _warnings = new List<ValidationResultItem>();

        public TValue? Value { get; set; } //iako je naglaseno da je klasa, a znamo da u domainu ce biti ID, mi smo napravili klasu successpostreposne koja ima ID polje
        public Guid RequestId { get; set; }
        public bool isAuthorized { get; set; } = true;

        public IReadOnlyList<ValidationResultItem> Errors => _errors.AsReadOnly();
        public IReadOnlyList<ValidationResultItem> Warnings => _warnings.AsReadOnly();
        public IReadOnlyList<ValidationResultItem> Info => _info.AsReadOnly();

        public bool HasError => _errors.Any();
        public bool HasWarning => _warnings.Any();
        public void AddError(ValidationResultItem item) => _errors.Add(item);
        public void AddWarning(ValidationResultItem item) => _warnings.Add(item);
        public void AddInfo(ValidationResultItem item) => _info.Add(item);
        //public IReadOnlyList<ValidationResultItem> Errors //kad idemo dohvatit errore,get vratimo readonly listu, za postavit vrijednost addrange
        //{
        //    get => _errors.AsReadOnly();
        //    init => _errors.AddRange(value);
        //}
        //isto i za warning i info , ali zasto samo nema metoda u kojoj _errors.Add(ValidationResultItem item)
        /* public bool HasError => Errors.All(validationResult => validationResult.ValidationSeverity == Domain.Common.Validation.ValidationSeverity.Error);*/ //zasto all a ne any

        public void SetResult(TValue value) //nema upitnika moze puknit, mozda dodat ga ako si ga gori doda?    
        {
            Value = value; //vjv taj entitet s kojim se barata, posta, deleta, updeta...
        }

        public void SetValidationResult(ValidationResult validationResult)
        {
            //_errors?.AddRange(validationResult.ValidationItems.Where(x => x.ValidationSeverity == ValidationSeverity.Error).Select(x => ValidationResultItem.FormValidationItem(x)); //ovo je krivo sto je ode htio, linq i ove metode ugradene ja nemam pojma
            //    //zasto addrange a ne add 
            var errors = validationResult.ValidationItems
                .Where(x => x.ValidationSeverity == ValidationSeverity.Error) //filtriramo samo  one s errorima, lista objekata tipa message, code, error s tim propertijima
                .Select(x => ValidationResultItem.FromValidationItem(x)); //mapiramo  u ovaj objekt koji nam pase(isti), ovo vraca ovaj tip iz app validationresultitem
            _errors.AddRange(errors); //add rande dodaje vise necega, kolekciju objekata npr ne jedan samo
            //isto i za warning i info
                     
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
            isAuthorized = false;
        }
    }
}
