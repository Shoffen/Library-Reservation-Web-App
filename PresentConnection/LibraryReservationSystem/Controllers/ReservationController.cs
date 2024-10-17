using Microsoft.AspNetCore.Mvc;
using LibraryReservationSystem.Models;
using LibraryReservationSystem.Services;
using Microsoft.EntityFrameworkCore;

namespace LibraryReservationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationsController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

      
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservations()
        {
            var reservations = await _reservationService.GetReservationsAsync();
            return Ok(reservations);
        }

        
        [HttpGet("calculatePrice")]
        public ActionResult<double> CalculatePrice([FromQuery] int days, [FromQuery] bool isAudiobook, [FromQuery] bool quickPickup, [FromQuery] double serviceFee, [FromQuery] double pickupCost)
        {
            double price = _reservationService.CalculatePrice(days, isAudiobook, quickPickup, serviceFee, pickupCost);
            return Ok(price);
        }

        
        [HttpPost]
        public async Task<ActionResult<Reservation>> CreateReservation(Reservation reservation)
        {
            await _reservationService.AddReservationAsync(reservation);
            return CreatedAtAction(nameof(GetReservation), new { id = reservation.Id }, reservation);
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<Reservation>> GetReservation(int id)
        {
            var reservation = await _reservationService.GetReservationByIdAsync(id);

            if (reservation == null)
            {
                return NotFound();
            }

            return Ok(reservation);
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            var reservation = await _reservationService.GetReservationByIdAsync(id);

            if (reservation == null)
            {
                return NotFound();
            }

            await _reservationService.DeleteReservationAsync(reservation);
            return NoContent();
        }

       
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReservation(int id, Reservation updatedReservation)
        {
            if (id != updatedReservation.Id)
            {
                return BadRequest();
            }

            try
            {
                await _reservationService.UpdateReservationAsync(updatedReservation);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _reservationService.GetReservationByIdAsync(id) == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
    }
}
