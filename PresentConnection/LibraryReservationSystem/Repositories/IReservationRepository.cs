using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryReservationSystem.Models;

public interface IReservationRepository
{   Task <List<Reservation>> Get();
    Task <Reservation?> GetReservationById(int id);
    Task AddReservation(Reservation reservation);
    Task UpdateReservation(Reservation reservation);
    Task DeleteReservation(Reservation reservation);

}