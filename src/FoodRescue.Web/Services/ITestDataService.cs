using FoodRescue.Web.Models;

namespace FoodRescue.Web.Services;

public interface ITestDataService
{
    List<FoodDonation> GenerateFoodDonations(int count = 10);
}
