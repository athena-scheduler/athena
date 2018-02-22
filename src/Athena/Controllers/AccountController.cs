using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Athena.Core.Models;
using Athena.Core.Models.Identity;
using Athena.Core.Repositories;
using Athena.Models.Login;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Athena.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly IStudentRepository _students;
        private readonly UserManager<AthenaUser> _userManager;
        private readonly SignInManager<AthenaUser> _signInManager;
        private readonly ILogger _log = Log.ForContext<AccountController>();

        public AccountController(
            IStudentRepository students,
            UserManager<AthenaUser> userManager,
            SignInManager<AthenaUser> signInManager
        )
        {
            _students = students ?? throw new ArgumentNullException(nameof(students));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        [HttpGet]
        [HttpGet("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            return View(new LoginViewModel
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Logout()
        {
            _log.Information("{user} logged out", User.Identity.Name);
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(HomeController.Home), "Home");
        }

        [HttpGet("{provider}")]
        [HttpPost("{provider}")]
        [AllowAnonymous]
        public IActionResult LoginWithExternalProvider(string provider, string returnUrl = null)
        {
            var redirect = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var propperties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirect);
            return Challenge(propperties, provider);
        }

        [HttpGet("callback/{provider}")]
        [HttpPost("callback/{provider}")]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                _log.Error("External login failed with {reoteError}", remoteError);
                return RedirectToAction(nameof(Login));
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                _log.Error("External auth provider didn't return any login info");
                return RedirectToAction(nameof(Login));
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey,
                isPersistent: true, bypassTwoFactor: true);

            if (result.Succeeded)
            {
                _log.Information("Login from {user} via {provider}", email, info.LoginProvider);
                return RedirectToLocal(returnUrl);
            }

            if (result.IsLockedOut)
            {
                _log.Warning("{user} tried to login but is locked out", email);
                
                // TODO: Display some sort of error to the user?
                return RedirectToAction(nameof(Login));
            }
            
            // The user does not have an account, we need to create one for them
            return View("Create", new CreateUserViewModel
            {
                Name = info.Principal.Identity.Name,
                Email = email,
                LoginProvider = info.ProviderDisplayName,
                ReturnUrl = returnUrl
            });
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateAccount(CreateUserViewModel model, string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToLocal(returnUrl);
            }

            var student = new Student
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Email = model.Email
            };

            await _students.AddAsync(student);

            if (ModelState.IsValid)
            {
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    throw new ApplicationException("Error confirming external user");
                }

                var user = new AthenaUser
                {
                    Id = student.Id,
                    UserName = model.Email,
                    Email = model.Email,
                    Student = student
                };

                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        
                        _log.Information("{user} created an account via {provider}", info.Principal.Identity.Name, info.ProviderDisplayName);

                        return RedirectToLocal(returnUrl);
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View("Create", model);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Home), "Home");
            }
        }
    }
}