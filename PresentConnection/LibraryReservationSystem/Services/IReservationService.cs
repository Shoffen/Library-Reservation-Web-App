using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryReservationSystem.Services
{
    public interface IReservationService
    {
        double CalculatePrice(int days, bool isAudiobook, bool quickPickup, double serviceFee, double pickupCost);
    }

}