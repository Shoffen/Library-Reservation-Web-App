using System;
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
        public string ImageUrl { get; set; } = string.Empty; 

        public bool IsAudiobook { get; set; }

        
        public Reservation() { }

       
        public Reservation(int bookId, DateTime startDate, DateTime endDate, bool isQuickPickup, double reservationCost, string imageUrl, bool isAudiobook)
        {
            BookId = bookId;
            StartDate = startDate;
            EndDate = endDate;
            IsQuickPickup = isQuickPickup;
            ReservationCost = reservationCost;
            ImageUrl = imageUrl ?? throw new ArgumentNullException(nameof(imageUrl));
            IsAudiobook = isAudiobook;
        }
    }
}
