using FoodRescue.Web.Authentication;
using FoodRescue.Web.Components;
using FoodRescue.Web.Configuration;
using FoodRescue.Web.Repositories;
using FoodRescue.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// AuthN/Z configuration (environment-aware)
builder.Services.AddAppAuthentication(builder.Configuration, builder.Environment);
builder.Services.AddAppAuthorization();

// Options/config binding
builder.Services.Configure<FeatureFlags>(builder.Configuration.GetSection("FeatureFlags"));

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<IDatabaseService, DatabaseService>();
builder.Services.AddScoped<IFoodDonationRepository, FoodDonationRepository>();
builder.Services.AddScoped<ITestDataService, TestDataService>();
builder.Services.AddScoped<FoodAnalysisStore>();
builder.Services.AddScoped<ReservationService>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["ApiBaseAddress"] ?? "http://localhost:5139") });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

// AuthN/Z middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

using (var scope = app.Services.CreateScope())
{
    var testDataService = scope.ServiceProvider.GetRequiredService<ITestDataService>();
    testDataService.GenerateFoodDonations();
}

app.Run();
