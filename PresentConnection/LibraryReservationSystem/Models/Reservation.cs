using System.ComponentModel.DataAnnotations;

namespace LibraryReservationSystem.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        [Required]
        public int BookId { get; set; }


        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public bool IsQuickPickup { get; set; }
        
        public double ReservationCost { get; set; }

        [Required]
        [StringLength(255)]
        public string? ImageUrl { get; set; } 


        public bool IsAudiobook { get; set; }

      
    }
}
