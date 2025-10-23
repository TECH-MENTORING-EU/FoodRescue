namespace FoodRescue.Web.Models
{
    public class FoodAnalysisResult
    {
        public string ImageBase64 { get; set; } = string.Empty;
        public string Caption { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
