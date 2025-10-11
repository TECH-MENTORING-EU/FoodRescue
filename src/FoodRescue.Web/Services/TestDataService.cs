using Bogus;
using FoodRescue.Web.Models;

namespace FoodRescue.Web.Services;

public class TestDataService : ITestDataService
{
    private readonly ILogger<TestDataService> _logger;

    public TestDataService(ILogger<TestDataService> logger)
    {
        _logger = logger;
    }

    public List<FoodDonation> GenerateFoodDonations(int count = 10)
    {
        _logger.LogInformation("Generating {Count} test food donations", count);

        var faker = new Faker<FoodDonation>()
            .RuleFor(f => f.Id, f => f.IndexFaker + 1)
            .RuleFor(f => f.DonorName, f => f.Company.CompanyName())
            .RuleFor(f => f.FoodType, f => f.PickRandom(new[] {
                "Bread", "Vegetables", "Fruits", "Dairy", "Canned Goods",
                "Prepared Meals", "Bakery Items", "Meat", "Beverages"
            }))
            .RuleFor(f => f.Quantity, f => f.Random.Int(1, 100))
            .RuleFor(f => f.Unit, f => f.PickRandom(new[] { "kg", "units", "boxes", "bags" }))
            .RuleFor(f => f.DonationDate, f => f.Date.Recent(30))
            .RuleFor(f => f.PickupLocation, f => f.Address.FullAddress())
            .RuleFor(f => f.IsPickedUp, f => f.Random.Bool());

        return faker.Generate(count);
    }
}
