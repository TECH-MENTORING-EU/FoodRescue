using FoodRescue.Web.Models;

namespace FoodRescue.Web.Repositories;

public interface IFoodDonationRepository
{
    Task<IEnumerable<FoodDonation>> GetAllAsync();
    Task<FoodDonation?> GetByIdAsync(int id);
    Task<int> CreateAsync(FoodDonation donation);
    Task<bool> UpdateAsync(FoodDonation donation);
    Task<bool> DeleteAsync(int id);
}
