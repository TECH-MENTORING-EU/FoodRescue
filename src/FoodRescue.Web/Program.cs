using FoodRescue.Web.Components;
using FoodRescue.Web.Services;
using FoodRescue.Web.Repositories;
using Microsoft.AspNetCore.Authentication;
using FoodRescue.Web.Authentication;

var builder = WebApplication.CreateBuilder(args);

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
