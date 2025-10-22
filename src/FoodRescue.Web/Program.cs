using FoodRescue.Web.Components;
using FoodRescue.Web.Services;
using FoodRescue.Web.Repositories;
using Microsoft.AspNetCore.Authentication;
using FoodRescue.Web.Authentication;
using Microsoft.AspNetCore.Authentication.Negotiate;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
if (builder.Environment.IsDevelopment())
{
    // Use the dev authentication scheme during development
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = "DevAuth";
        options.DefaultChallengeScheme = "DevAuth";
    }).AddDevAuthentication();
}
else
{
    // Use Windows Authentication in other environments
    builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
        .AddNegotiate();
}


builder.Services.AddAuthorization(options =>
{
    // Sets the fallback policy to require authenticated users for all pages.
    options.FallbackPolicy = options.DefaultPolicy;
});

// For Blazor to expose the authentication state as a cascading parameter.
builder.Services.AddCascadingAuthenticationState();

// Add the HttpContextAccessor service for accessing the user's identity.
builder.Services.AddHttpContextAccessor();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<IDatabaseService, DatabaseService>();
builder.Services.AddScoped<IFoodDonationRepository, FoodDonationRepository>();
builder.Services.AddScoped<ITestDataService, TestDataService>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["ApiBaseAddress"] ?? "http://localhost:5139") });

builder.Services.AddAuthentication("DevScheme")
    .AddScheme<AuthenticationSchemeOptions, DevAuthenticationHandler>("DevScheme", null);


var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();




// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

using (var scope = app.Services.CreateScope())
{
    var testDataService = scope.ServiceProvider.GetRequiredService<ITestDataService>();
    testDataService.GenerateFoodDonations(); // Replace Seed() with GenerateFoodDonations()
}

app.Run();
