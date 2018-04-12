using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Athena.Exceptions
{
    public class BadModelException : ApiException
    {
        public readonly ModelStateDictionary ModelState;

        public BadModelException(ModelStateDictionary state) : base(
            HttpStatusCode.BadRequest,
            "Validation of one or more parameters failed",
            state.Keys.Select(
                k => new {Key = k, Error = state[k].Errors.Select(er => er.ErrorMessage)}
            ).ToDictionary(k => k.Key, v => v.Error)
        ) => ModelState = state;
    }
}