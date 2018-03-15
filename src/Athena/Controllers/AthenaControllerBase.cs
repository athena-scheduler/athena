using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Athena.Controllers
{
    [Authorize]
    public abstract class AthenaControllerBase : Controller
    {
    }
}