using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace FoodRescue.Web.Authentication;

public class DevAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public DevAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // Auto-authenticate in development mode
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "Developer"),
            new Claim(ClaimTypes.Email, "dev@foodrescue.local"),
            new Claim(ClaimTypes.Role, "Developer"),
        };

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}

public static class DevAuthenticationExtensions
{
    public static AuthenticationBuilder AddDevAuthentication(this AuthenticationBuilder builder)
    {
        return builder.AddScheme<AuthenticationSchemeOptions, DevAuthenticationHandler>(
            "DevAuth", options => { });
    }
}
