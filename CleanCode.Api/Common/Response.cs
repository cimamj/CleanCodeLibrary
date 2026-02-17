using CleanCodeLibrary.Application.Common.Model;

namespace CleanCode.Api.Common
{
    public class Response<TValue> where TValue : class
    {
        private IReadOnlyList<ValidationResultItem> _info { get; init; }
        private IReadOnlyList<ValidationResultItem> _errors { get; init; }
        private IReadOnlyList<ValidationResultItem> _warnings { get; init; }

        public IReadOnlyList<ValidationResultItem> Errors => _errors;

        public IReadOnlyList<ValidationResultItem> Warnings => _warnings;
        public IReadOnlyList<ValidationResultItem> Info => _info;

        public bool isAuthorized { get; set; }
        public TValue? Value { get; set; } 
        public Guid RequestId { get; set; }

        public Response(Result<TValue> result) 
        { 
            _info = result.Info;
            _errors = result.Errors;
            _warnings = result.Warnings;
            isAuthorized = result.isAuthorized; //dodano

            Value = result.Value;
            RequestId = result.RequestId;   
        }

        public bool HasError => _errors.Any(); //on ovo nema, kako onda iz ext ocita, svojstvo
        public bool HasValue => Value != null;

    }
}
