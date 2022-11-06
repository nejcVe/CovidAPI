using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CovidAPI.Services {
    public class BasicAuthenticationService : AuthenticationHandler<AuthenticationSchemeOptions> {

        public BasicAuthenticationService(
             IOptionsMonitor<AuthenticationSchemeOptions> options,
             ILoggerFactory logger,
             UrlEncoder encoder,
             ISystemClock clock) :
             base(options, logger, encoder, clock) {
        }
        protected async override Task<AuthenticateResult> HandleAuthenticateAsync() {
            var authorizationHeader = Request.Headers["Authorization"].ToString();
            if (authorizationHeader != null && authorizationHeader.StartsWith("basic", StringComparison.OrdinalIgnoreCase)) {
                var b64string = authorizationHeader.Substring(6).Trim();
                var encodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(b64string));
                var credentials = encodedCredentials.Split(":");
                Console.WriteLine(b64string);
                if (credentials[0] == "user" && credentials[1] == "covid_api_password") {
                    var claims = new[] {
                        new Claim("name", credentials[0]),
                        new Claim(ClaimTypes.Role, "Admin")
                    };
                    var identity = new ClaimsIdentity(claims, "Basic");
                    var claimsPrincipal = new ClaimsPrincipal(identity);
                    return await Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name)));
                }
            }
            Response.StatusCode = 401;
            Response.Headers.Add("WWW-Authenticate", "Basic realm=\"CovidAPI\"");
            return await Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
        }
    }
}
