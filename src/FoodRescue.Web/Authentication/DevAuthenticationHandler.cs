using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace FoodRescue.Web.Authentication;

public class DevAuthOptions
{
    public string Name { get; set; } = "Developer";
    public string Email { get; set; } = "dev@foodrescue.local";
    public string[] Roles { get; set; } // default roles for local testing
}

public class DevAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IOptions<DevAuthOptions> _options;

    public DevAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IOptions<DevAuthOptions> devOptions)
        : base(options, logger, encoder)
    {
        _options = devOptions;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var dev = _options.Value;

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, dev.Name),
            new Claim(ClaimTypes.Email, dev.Email)
        };

        foreach (var role in dev.Roles ?? Array.Empty<string>())
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        // Explicitly set role claim type
        var identity = new ClaimsIdentity(
            claims,
            authenticationType: Scheme.Name,
            nameType: ClaimTypes.Name,
            roleType: ClaimTypes.Role);

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
