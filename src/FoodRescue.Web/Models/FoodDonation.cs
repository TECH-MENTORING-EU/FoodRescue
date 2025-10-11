namespace FoodRescue.Web.Models;

public class FoodDonation
{
    public int Id { get; set; }
    public string DonorName { get; set; } = string.Empty;
    public string FoodType { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string Unit { get; set; } = string.Empty;
    public DateTime DonationDate { get; set; }
    public string PickupLocation { get; set; } = string.Empty;
    public bool IsPickedUp { get; set; }
}
