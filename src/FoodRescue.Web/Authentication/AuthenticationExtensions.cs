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
            // Production: Use Azure App Service Easy Auth headers
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = AzureEasyAuthDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = AzureEasyAuthDefaults.AuthenticationScheme;
            })
            .AddScheme<AuthenticationSchemeOptions, AzureEasyAuthHandler>(
                AzureEasyAuthDefaults.AuthenticationScheme,
                _ => { });
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
