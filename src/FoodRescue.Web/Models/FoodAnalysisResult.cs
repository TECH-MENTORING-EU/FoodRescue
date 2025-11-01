using static FoodRescue.Web.Components.Pages.FoodDetails;

namespace FoodRescue.Web.Models
{
    public class FoodAnalysisResult
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ImageBase64 { get; set; } = string.Empty;

        public string Caption { get; set; } = string.Empty;

        public string JsonTable { get; set; } = string.Empty;


        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public List<FoodItem> FoodItems { get; set; } = new();
    }
}
