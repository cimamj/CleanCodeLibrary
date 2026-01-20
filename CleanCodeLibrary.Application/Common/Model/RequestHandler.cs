

namespace CleanCodeLibrary.Application.Common.Model
{
    public abstract class RequestHandler<TRequest, TResult> where TRequest : class where TResult : class
    {
        //nastavi ode pa ces skuzit validaciju mapiranje iz domaina u app POSLOZIT CE SE SVE
        //kako kad stvori zna jel false ili true isAuthorized na Jure MAmic 15.5.
        public Guid RequestId => Guid.NewGuid();

        //dakle request i response triba ASYNC metoda jer ce ic na bazu api call, vraca result koji barata s TResult (neki entitet)
        public async Task<Result<TResult>> ProcessAuthorizedRequestAsync(TRequest request)
        {
            var result = new Result<TResult>
            {
                RequestId = RequestId
            };


            if (await IsAuthorized() == false) 
            {
                result.SetUnauthorizedResult();  //ali je li result dobije value svoj onda?, zapravo value je ovaj TResult tj SuccessPostResponse koji ima id
                return result; 
            }

            await HandleRequest(request, result);//zasto handle vraca ista, onda bi se valjda spremalo u nesto

            return result; //kad ovo zapravo vraca , sta ovo vraca 2 puta
        }

        protected abstract Task<Result<TResult>> HandleRequest(TRequest request, Result<TResult> result);
        protected abstract Task<bool> IsAuthorized();
    }
}
