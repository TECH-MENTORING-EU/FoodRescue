namespace FoodRescue.Web.Models
{
    public class FoodReservation
    {
        public Guid AnalysisId { get; set; }
        public string Product { get; set; } = "";
        public int ReservedAmount { get; set; }
        public string UserId { get; set; } = "";
        public DateTime ReservedAt { get; set; } = DateTime.UtcNow; 
    }
}
