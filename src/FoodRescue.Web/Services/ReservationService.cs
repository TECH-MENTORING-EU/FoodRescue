using FoodRescue.Web.Models;

namespace FoodRescue.Web.Services
{
    public class ReservationService
    {
        private readonly List<FoodReservation> _reservations = new();

        public IReadOnlyList<FoodReservation> Reservations => _reservations;

        public void AddReservation(FoodReservation reservation)
        {
            _reservations.Add(reservation);
        }

        public List<FoodReservation> viewReservations(string UserId)
        {
            List<FoodReservation> userReservations = _reservations.FindAll(r => r.UserId == UserId);
            return (userReservations);
        }
    }
}