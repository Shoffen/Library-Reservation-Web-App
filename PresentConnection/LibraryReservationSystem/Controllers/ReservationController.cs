using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryReservationSystem.Data;
using LibraryReservationSystem.Models;

namespace LibraryReservationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly LibraryContext _context;

        public ReservationsController(LibraryContext context)
        {
            _context = context;
        }

        // GET: api/reservations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservations()
        {
            
            return await _context.Reservations.ToListAsync();
        }

        // POST: api/reservations/calculatePrice
        [HttpGet("calculatePrice")]
        public ActionResult<double> CalculatePrice([FromQuery] int days, [FromQuery] bool isAudiobook, [FromQuery] bool quickPickup, [FromQuery] double serviceFee, [FromQuery] double pickupCost)
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

        // POST: api/reservations
        [HttpPost]
        public async Task<ActionResult<Reservation>> CreateReservation(Reservation reservation)
        {
            // Check if the book exists
            var bookExists = await _context.Books.AnyAsync(b => b.Id == reservation.BookId);
            if (!bookExists)
            {
                return BadRequest("The specified book does not exist.");
            }

            // Check for model state errors
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return BadRequest(ModelState);
            }

            if (reservation.StartDate >= reservation.EndDate)
            {
                return BadRequest("End date must be later than start date.");
            }

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReservations), new { id = reservation.Id }, reservation);
        }

    }
}
