using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryReservationSystem.Data;
using LibraryReservationSystem.Models;
using LibraryReservationSystem.Repositories;
using LibraryReservationSystem.Services;

namespace LibraryReservationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IReservationService _reservationService;

        public ReservationsController (IReservationRepository reservationRepository, IReservationService reservationService)
        {
            _reservationRepository = reservationRepository;
            _reservationService = reservationService;
        }

        // GET: api/reservations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservations()
        {
            
            return await _reservationRepository.Get();
        }

        // POST: api/reservations/calculatePrice
        [HttpGet("calculatePrice")]
        public ActionResult<double> CalculatePrice([FromQuery] int days, [FromQuery] bool isAudiobook, [FromQuery] bool quickPickup, [FromQuery] double serviceFee, [FromQuery] double pickupCost)
        {
            double price = _reservationService.CalculatePrice(days, isAudiobook, quickPickup, serviceFee, pickupCost);
            return Ok(price);
        }

        // POST: api/reservations
        [HttpPost]
        public async Task<ActionResult<Reservation>> CreateReservation(Reservation reservation)
        {
            await _reservationRepository.AddReservation(reservation);
            return CreatedAtAction(nameof(GetReservation), new { id = reservation.Id }, reservation);
        }

        // GET: api/reservation/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Reservation>> GetReservation(int id)
        {
            var reservation = await _reservationRepository.GetReservationById(id);

            if (reservation == null)
            {
                return NotFound();
            }

            return Ok(reservation);
        }

    }
}
