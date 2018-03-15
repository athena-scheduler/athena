using System.Security.Principal;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Athena.Core.Models.Identity;
using Athena.Models.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;

namespace Athena.Handlers
{
    public class ApiKeyHandler : AuthenticationHandler<ApiKeyHandler._>
    {
        public class _ : AuthenticationSchemeOptions
        {
        }
        
        public const string SCHEME = "api-key";
        public const string ATHENA_API_HEADER_KEY = "X-ATHENA-API-KEY";

        private readonly IUserApiKeyStore _apiKeys;

        public ApiKeyHandler(
            IUserApiKeyStore store,
            IOptionsMonitor<_> options,
            ILoggerFactory logger, UrlEncoder encoder,
            ISystemClock clock
        ) : base(options, logger, encoder, clock) => _apiKeys = store;

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (Request.Headers.TryGetValue(ATHENA_API_HEADER_KEY, out var apiKey))
            {
                var user = await _apiKeys.GetUserForApiKey(apiKey);

                if (user == null)
                {
                    return AuthenticateResult.Fail("API Key Not Found");
                }

                return AuthenticateResult.Success(
                    new AuthenticationTicket(
                        new AthenaPrincipal(
                            new GenericPrincipal(new GenericIdentity(user.UserName), null),
                            user
                        ), 
                        SCHEME
                    )
                );
            }
            
            return AuthenticateResult.Fail($"Missing {ATHENA_API_HEADER_KEY} header");
        }
    }
}