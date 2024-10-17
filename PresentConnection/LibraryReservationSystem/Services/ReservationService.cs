using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryReservationSystem.Services
{
    public class ReservationService : IReservationService
    {
        public double CalculatePrice(int days, bool isAudiobook, bool quickPickup, double serviceFee, double pickupCost)
        {
            double dailyRate = isAudiobook ? 2.0 : 3.0; 
            double subtotal = dailyRate * days;
            double totalCost = subtotal + serviceFee;

            if (quickPickup)
            {
                totalCost += pickupCost; 
            }

            
            if (days >= 3 && days < 10)
            {
                totalCost -= subtotal * 0.1; 
            }
            else if (days >= 10)
            {
                totalCost -= subtotal * 0.2; 
            }

            return totalCost;
        }
    }
}