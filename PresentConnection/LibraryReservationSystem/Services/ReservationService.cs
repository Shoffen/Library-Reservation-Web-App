using LibraryReservationSystem.Models;
using LibraryReservationSystem.Repositories;

namespace LibraryReservationSystem.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;

        public ReservationService(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        
        public double CalculatePrice(int days, bool isAudiobook, bool quickPickup, double serviceFee, double pickupCost)
        {
            double dailyRate = isAudiobook ? 2.0 : 3.0; 
            double subtotal = dailyRate * days;
            double totalCost = subtotal + serviceFee;


            if (days >= 3 && days < 10)
            {
                totalCost -= subtotal * 0.1; 
            }
            else if (days >= 10)
            {
                totalCost -= subtotal * 0.2; 
            }

             if (quickPickup)
            {
                totalCost += pickupCost; 
            }

            return totalCost;
        }
        public async Task<List<Reservation>> GetReservationsAsync()
        {
            return await _reservationRepository.Get();
        }

        public async Task<Reservation?> GetReservationByIdAsync(int id)
        {
            return await _reservationRepository.GetReservationById(id);
        }

        public async Task AddReservationAsync(Reservation reservation)
        {
            await _reservationRepository.AddReservation(reservation);
        }

        public async Task DeleteReservationAsync(Reservation reservation)
        {
            await _reservationRepository.DeleteReservation(reservation);
        }

        public async Task UpdateReservationAsync(Reservation reservation)
        {
            await _reservationRepository.UpdateReservation(reservation);
        }
    }
}
