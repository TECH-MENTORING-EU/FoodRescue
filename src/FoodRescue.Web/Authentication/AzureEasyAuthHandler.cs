using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace FoodRescue.Web.Authentication;

public static class AzureEasyAuthDefaults
{
    public const string AuthenticationScheme = "AzureEasyAuth";

    public const string PrincipalHeader = "X-MS-CLIENT-PRINCIPAL";
    public const string PrincipalNameHeader = "X-MS-CLIENT-PRINCIPAL-NAME";
}

/// <summary>
/// Authentication handler for Azure App Service Easy Auth. It reads claims from injected headers.
/// </summary>
public sealed class AzureEasyAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public AzureEasyAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        System.Text.Encodings.Web.UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(AzureEasyAuthDefaults.PrincipalHeader, out var headerValues) || headerValues.Count == 0)
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        try
        {
            var header = headerValues[0];
            var decoded = Convert.FromBase64String(header);
            var json = Encoding.UTF8.GetString(decoded);

            var clientPrincipal = JsonSerializer.Deserialize<ClientPrincipal>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (clientPrincipal is null || clientPrincipal.Claims is null || clientPrincipal.Claims.Count == 0)
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            string nameType = string.IsNullOrWhiteSpace(clientPrincipal.NameType) ? ClaimTypes.Name : clientPrincipal.NameType!;
            string roleType = string.IsNullOrWhiteSpace(clientPrincipal.RoleType) ? ClaimTypes.Role : clientPrincipal.RoleType!;

            var claims = new List<Claim>(clientPrincipal.Claims.Count + 2);
            foreach (var c in clientPrincipal.Claims)
            {
                if (!string.IsNullOrEmpty(c.Typ) && c.Val is not null)
                {
                    claims.Add(new Claim(c.Typ!, c.Val));
                }
            }

            if (Request.Headers.TryGetValue(AzureEasyAuthDefaults.PrincipalNameHeader, out var nameHeader) &&
                !string.IsNullOrWhiteSpace(nameHeader) &&
                !claims.Any(c => c.Type == nameType))
            {
                claims.Add(new Claim(nameType, nameHeader.ToString()));
            }

            var identity = new ClaimsIdentity(claims, AzureEasyAuthDefaults.AuthenticationScheme, nameType, roleType);
            var principal = new ClaimsPrincipal(identity);

            var ticket = new AuthenticationTicket(principal, new AuthenticationProperties
            {
                IsPersistent = true
            }, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to parse Azure Easy Auth headers.");
            return Task.FromResult(AuthenticateResult.Fail("Invalid Easy Auth headers"));
        }
    }

    protected override Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        // Redirect to Easy Auth login endpoint (AAD by default).
        var returnUrl = Uri.EscapeDataString(Request.Path + Request.QueryString);
        Response.StatusCode = StatusCodes.Status302Found;
        Response.Headers.Location = $"/.auth/login/aad?post_login_redirect_uri={returnUrl}";
        return Task.CompletedTask;
    }

    protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
    {
        Response.StatusCode = StatusCodes.Status403Forbidden;
        return Task.CompletedTask;
    }

    private sealed class ClientPrincipal
    {
        public string? AuthType { get; set; }
        public string? NameType { get; set; }
        public string? RoleType { get; set; }
        public List<ClientClaim> Claims { get; set; } = new();
    }

    private sealed class ClientClaim
    {
        public string? Typ { get; set; }
        public string? Val { get; set; }
    }
}
