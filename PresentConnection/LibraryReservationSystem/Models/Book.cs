using System;

namespace LibraryReservationSystem.Models
{
    public class Book
    {
        public int Id { get; set; }             
        public string Name { get; set; }        
        public int Year { get; set; }           
        public bool Audiobook { get; set; }     
        public bool PhysicalBook { get; set; }  
        public string ImageUrl { get; set; }    

        
        public Book(string name, int year, bool audiobook, bool physicalBook, string imageUrl)
        {
            Name = name;
            Year = year;
            Audiobook = audiobook;
            PhysicalBook = physicalBook;
            ImageUrl = imageUrl;
        }

       
    }
}
