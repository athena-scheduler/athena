using Athena.Filters;
using Athena.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Athena.Controllers.api
{
    [ApiModelValidation]
    [Authorize(AuthenticationSchemes = ApiKeyHandler.SCHEME)]
    public class AthenaApiController : Controller
    {
    }
}