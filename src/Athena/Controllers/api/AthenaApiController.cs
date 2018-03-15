using Athena.Filters;
using Athena.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Athena.Controllers.api
{
    [ApiModelValidation]
    [Authorize]
    public class AthenaApiController : Controller
    {
    }
}