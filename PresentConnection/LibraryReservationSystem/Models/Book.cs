using System.ComponentModel.DataAnnotations;

public class Book
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty; 

    public int Year { get; set; }
    public bool Audiobook { get; set; }
    public bool PhysicalBook { get; set; }

    [Required] 
    public string ImageUrl { get; set; } = string.Empty; 

    public Book() { }

    public Book(string name, int year, bool audiobook, bool physicalBook, string imageUrl)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Year = year;
        Audiobook = audiobook;
        PhysicalBook = physicalBook;
        ImageUrl = imageUrl ?? throw new ArgumentNullException(nameof(imageUrl));
    }
}
