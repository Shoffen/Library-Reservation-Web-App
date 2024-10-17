using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryReservationSystem.Data;
using LibraryReservationSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryReservationSystem.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly LibraryContext _context;

        public ReservationRepository(LibraryContext _Context) =>
            _context = _Context;

        public async Task AddReservation(Reservation reservation)
        {
            await _context.Reservations.AddAsync(reservation);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteReservation(Reservation reservation)
        {
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Reservation>> Get()
        {
            return await _context.Reservations.ToListAsync();
        }

        public async Task<Reservation?> GetReservationById(int id)
        {
            return await _context.Reservations.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task UpdateReservation (Reservation reservation)
        {
            _context.Reservations.Update(reservation);
            await _context.SaveChangesAsync();
        }
    }
}