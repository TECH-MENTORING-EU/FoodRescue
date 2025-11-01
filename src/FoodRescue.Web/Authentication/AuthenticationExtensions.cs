using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace FoodRescue.Web.Authentication;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddAppAuthentication(this IServiceCollection services, IConfiguration config, IHostEnvironment env)
    {
        var useDevAuth = env.IsDevelopment();

        if (useDevAuth)
        {
            services.Configure<DevAuthOptions>(config.GetSection("DevAuth"));

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "DevAuth";
                options.DefaultChallengeScheme = "DevAuth";
            }).AddDevAuthentication();
        }
        else
        {
            var tenantId = config["EntraId:TenantId"];
            var clientId = config["EntraId:ClientId"];
            var clientSecret = config["EntraId:ClientSecret"];
            var callbackPath = config["EntraId:CallbackPath"] ?? "/signin-oidc";

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.SlidingExpiration = true;
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            })
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.Authority = $"https://login.microsoftonline.com/{tenantId}/v2.0";
                options.ClientId = clientId;
                options.ClientSecret = clientSecret;
                options.CallbackPath = callbackPath;

                options.ResponseType = OpenIdConnectResponseType.Code;
                options.UsePkce = true;
                options.SaveTokens = false;

                options.Scope.Clear();
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("email");

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name",
                    RoleClaimType = "roles"
                };

                options.ClaimActions.MapUniqueJsonKey(ClaimTypes.NameIdentifier, "sub");
                options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
                options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
                options.ClaimActions.MapJsonKey(ClaimTypes.Role, "roles");
            });
        }

        return services;
    }

    public static IServiceCollection AddAppAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            // Authenticated-only fallback. Roles are enforced per-page/component.
            options.FallbackPolicy = options.DefaultPolicy;

            options.AddPolicy("DeveloperOnly", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireRole("Developer");
            });
        });

        services.AddCascadingAuthenticationState();
        services.AddHttpContextAccessor();

        return services;
    }
}
