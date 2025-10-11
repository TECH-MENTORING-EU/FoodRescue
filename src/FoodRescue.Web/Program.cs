using FoodRescue.Web.Authentication;
using FoodRescue.Web.Components;
using FoodRescue.Web.Repositories;
using FoodRescue.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register database and repository services
builder.Services.AddSingleton<IDatabaseService, DatabaseService>();
builder.Services.AddScoped<IFoodDonationRepository, FoodDonationRepository>();
builder.Services.AddScoped<ITestDataService, TestDataService>();

// Configure authentication - use dev auth in development
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddAuthentication("DevAuth")
        .AddDevAuthentication();
}
else
{
    // In production, you would configure proper authentication here
    // For example: Azure AD, IdentityServer, or ASP.NET Core Identity
    builder.Services.AddAuthentication("DevAuth")
        .AddDevAuthentication();
}

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
