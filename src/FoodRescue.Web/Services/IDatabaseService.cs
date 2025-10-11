using System.Data;

namespace FoodRescue.Web.Services;

public interface IDatabaseService
{
    IDbConnection CreateConnection();
}
