using Dapper;
using FoodRescue.Web.Models;
using FoodRescue.Web.Services;

namespace FoodRescue.Web.Repositories;

public class FoodDonationRepository : IFoodDonationRepository
{
    private readonly IDatabaseService _databaseService;
    private readonly ILogger<FoodDonationRepository> _logger;

    public FoodDonationRepository(IDatabaseService databaseService, ILogger<FoodDonationRepository> logger)
    {
        _databaseService = databaseService;
        _logger = logger;
    }

    public async Task<IEnumerable<FoodDonation>> GetAllAsync()
    {
        _logger.LogDebug("Getting all food donations");
        using var connection = _databaseService.CreateConnection();
        var sql = "SELECT * FROM FoodDonations";
        return await connection.QueryAsync<FoodDonation>(sql);
    }

    public async Task<FoodDonation?> GetByIdAsync(int id)
    {
        _logger.LogDebug("Getting food donation with ID: {Id}", id);
        using var connection = _databaseService.CreateConnection();
        var sql = "SELECT * FROM FoodDonations WHERE Id = @Id";
        return await connection.QueryFirstOrDefaultAsync<FoodDonation>(sql, new { Id = id });
    }

    public async Task<int> CreateAsync(FoodDonation donation)
    {
        _logger.LogInformation("Creating new food donation from {DonorName}", donation.DonorName);
        using var connection = _databaseService.CreateConnection();
        var sql = @"
            INSERT INTO FoodDonations (DonorName, FoodType, Quantity, Unit, DonationDate, PickupLocation, IsPickedUp)
            VALUES (@DonorName, @FoodType, @Quantity, @Unit, @DonationDate, @PickupLocation, @IsPickedUp);
            SELECT CAST(SCOPE_IDENTITY() as int)";
        return await connection.ExecuteScalarAsync<int>(sql, donation);
    }

    public async Task<bool> UpdateAsync(FoodDonation donation)
    {
        _logger.LogInformation("Updating food donation with ID: {Id}", donation.Id);
        using var connection = _databaseService.CreateConnection();
        var sql = @"
            UPDATE FoodDonations
            SET DonorName = @DonorName,
                FoodType = @FoodType,
                Quantity = @Quantity,
                Unit = @Unit,
                DonationDate = @DonationDate,
                PickupLocation = @PickupLocation,
                IsPickedUp = @IsPickedUp
            WHERE Id = @Id";
        var affectedRows = await connection.ExecuteAsync(sql, donation);
        return affectedRows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        _logger.LogInformation("Deleting food donation with ID: {Id}", id);
        using var connection = _databaseService.CreateConnection();
        var sql = "DELETE FROM FoodDonations WHERE Id = @Id";
        var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
        return affectedRows > 0;
    }
}
