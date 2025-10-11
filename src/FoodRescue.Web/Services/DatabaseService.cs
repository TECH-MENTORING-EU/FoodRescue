using System.Data;
using Microsoft.Data.SqlClient;

namespace FoodRescue.Web.Services;

public class DatabaseService : IDatabaseService
{
    private readonly string _connectionString;
    private readonly ILogger<DatabaseService> _logger;

    public DatabaseService(IConfiguration configuration, ILogger<DatabaseService> logger)
    {
        _connectionString = configuration.GetConnectionString("FoodRescueDb") 
            ?? throw new InvalidOperationException("Connection string 'FoodRescueDb' not found.");
        _logger = logger;
    }

    public IDbConnection CreateConnection()
    {
        _logger.LogDebug("Creating database connection");
        return new SqlConnection(_connectionString);
    }
}
