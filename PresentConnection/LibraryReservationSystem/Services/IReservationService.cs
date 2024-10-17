using LibraryReservationSystem.Models;

namespace LibraryReservationSystem.Services
{
    public interface IReservationService
    {
        double CalculatePrice(int days, bool isAudiobook, bool quickPickup, double serviceFee, double pickupCost);
        Task<List<Reservation>> GetReservationsAsync();
        Task<Reservation?> GetReservationByIdAsync(int id);
        Task AddReservationAsync(Reservation reservation);
        Task UpdateReservationAsync(Reservation reservation);
        Task DeleteReservationAsync(Reservation reservation);
    }
}
