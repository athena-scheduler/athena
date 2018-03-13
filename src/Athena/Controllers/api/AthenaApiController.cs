using Athena.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Athena.Controllers.api
{
    [ApiModelValidation]
    [Authorize(AuthenticationSchemes = "api-key")]
    public class AthenaApiController : Controller
    {
    }
}