using Microsoft.EntityFrameworkCore;
using LibraryReservationSystem.Models;

namespace LibraryReservationSystem.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }

        // Add the DbSet for Reservations
        public DbSet<Reservation> Reservations { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().HasKey(b => b.Id);
            modelBuilder.Entity<Book>()
                .Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(255);
        }

        // Method to seed data
        public void SeedData()
        {
            if (!Books.Any())
            {   
                
                Books.AddRange(new List<Book>
                {
                    new Book {Name= "Metamorfozė", Year = 1925, Audiobook= true, PhysicalBook = false, ImageUrl = "https://blogger.googleusercontent.com/img/b/R29vZ2xl/AVvXsEjN4kbwat_hAakP5-OLaRykekLmcc_mL4VuD0hOU7pMGR_kLytLJV2FHMZSKe-rzBLzxXB7qIGPSt7mB1thk0A0X5yL85zbyAFX6nFqGiNnWsLl6mMeMmGOJO7DVeX_HIdVUK7I5BHsPH4/s1600/The-Metamorphosis-Kafka-Copy.png" },
                    new Book { Name = "Metai", Year = 1949, Audiobook = false, PhysicalBook = true, ImageUrl = "https://thumb.knygos-static.lt/1dNdTshCHxFVyKxMFL0rxoPtYG0=/fit-in/0x800/filters:cwatermark(static/wm.png,500,75,30)/images/books/12399/1462877473_59.jpg" },
                    new Book { Name = "Dėdės ir dėdienės", Year = 1940, Audiobook = false, PhysicalBook = true, ImageUrl = "https://thumb.knygos-static.lt/cOoUepuTHfA2xVE4Q-oG-aOze_o=/fit-in/0x800/filters:cwatermark(static/wm.png,500,75,30)/images/books/20928/1462881899_img0018.jpg" },
                    new Book { Name = "Hamletas", Year = 1619, Audiobook = false, PhysicalBook = true, ImageUrl = "https://www.patogupirkti.lt/media/catalog/product/cache/1/image/1800x/040ec09b1e35df139433887a97daa66f/h/a/hamletas012.jpg" },
                    new Book { Name = "Dievų miškas", Year = 1802, Audiobook = false, PhysicalBook = true, ImageUrl = "https://www.patogupirkti.lt/media/catalog/product/cache/1/image/1800x/040ec09b1e35df139433887a97daa66f/d/i/dievu_miskas.jpg" },
                    new Book { Name = "Sename Dvare", Year = 1967, Audiobook = true, PhysicalBook = false, ImageUrl = "https://thumb.knygos-static.lt/uNFswH_mAt_WPTW8rml1_hvuuMQ=/fit-in/0x800/filters:cwatermark(static/wm.png,500,75,30)/images/books/2661179/book-9861641913182.png" },
                    new Book { Name = "Anykščių šilelis", Year = 1962, Audiobook = true, PhysicalBook = true, ImageUrl = "https://cdn.kobo.com/book-images/55ffa513-8c64-4a1d-9c7a-18197abcd46f/353/569/90/False/anyksciu-silelis.jpg" },
                    new Book { Name = "Balta drobulė", Year = 1960, Audiobook = false, PhysicalBook = true, ImageUrl = "https://thumb.knygos-static.lt/J2GjLEQzE954NnogcKeI3Br6uZc=/fit-in/0x800/filters:cwatermark(static/wm.png,500,75,30)/images/books/2731004/1672655005_1670484351_1669877794_1462878414_9789955236627asbaltadrobule72m.jpg" },
                    new Book { Name = "Altorių šešėly", Year = 1965, Audiobook = true, PhysicalBook = false, ImageUrl = "https://thumb.knygos-static.lt/BkYKfT32Y7Fa9BOtJBYWQaikjSA=/fit-in/0x800/filters:cwatermark(static/wm.png,500,75,30)/images/books/744081/1462885400_vmpaltoriusesely72max.jpg" },
                    new Book { Name = "Faustas", Year = 1969, Audiobook = true, PhysicalBook = true, ImageUrl = "https://www.patogupirkti.lt/media/catalog/product/cache/1/image/1800x/040ec09b1e35df139433887a97daa66f/f/a/faustas-1.jpg" },
                    new Book { Name = "Dėdės ir dėdienės", Year = 1961, Audiobook = true, PhysicalBook = true, ImageUrl = "https://thumb.knygos-static.lt/cOoUepuTHfA2xVE4Q-oG-aOze_o=/fit-in/0x800/filters:cwatermark(static/wm.png,500,75,30)/images/books/20928/1462881899_img0018.jpg" },
                    new Book { Name = "Sename Dvare", Year = 1860, Audiobook = true, PhysicalBook = true, ImageUrl = "https://thumb.knygos-static.lt/uNFswH_mAt_WPTW8rml1_hvuuMQ=/fit-in/0x800/filters:cwatermark(static/wm.png,500,75,30)/images/books/2661179/book-9861641913182.png" },
                    new Book { Name = "Balta drobulė", Year = 1910, Audiobook = true, PhysicalBook = false, ImageUrl = "https://thumb.knygos-static.lt/J2GjLEQzE954NnogcKeI3Br6uZc=/fit-in/0x800/filters:cwatermark(static/wm.png,500,75,30)/images/books/2731004/1672655005_1670484351_1669877794_1462878414_9789955236627asbaltadrobule72m.jpg" },
                    new Book { Name = "Metai", Year = 1749, Audiobook = true, PhysicalBook = true, ImageUrl = "https://thumb.knygos-static.lt/1dNdTshCHxFVyKxMFL0rxoPtYG0=/fit-in/0x800/filters:cwatermark(static/wm.png,500,75,30)/images/books/12399/1462877473_59.jpg" },
                    new Book { Name = "Altorių šešėly", Year = 1945, Audiobook = false, PhysicalBook = true, ImageUrl = "https://thumb.knygos-static.lt/BkYKfT32Y7Fa9BOtJBYWQaikjSA=/fit-in/0x800/filters:cwatermark(static/wm.png,500,75,30)/images/books/744081/1462885400_vmpaltoriusesely72max.jpg" },
                    new Book { Name = "Hamletas", Year = 1819, Audiobook = true, PhysicalBook = true, ImageUrl = "https://www.patogupirkti.lt/media/catalog/product/cache/1/image/1800x/040ec09b1e35df139433887a97daa66f/h/a/hamletas012.jpg" },
                    new Book { Name = "Skirgaila", Year = 1632, Audiobook = false, PhysicalBook = true, ImageUrl = "https://thumb.knygos-static.lt/wpAL-BosJO8_XTfjaId-jR1cCi8=/fit-in/0x800/filters:cwatermark(static/wm.png,500,75,30)/images/books/2590268/ggss.jpg" },
                    new Book { Name = "Dievų miškas", Year = 1832, Audiobook = true, PhysicalBook = false, ImageUrl = "https://www.patogupirkti.lt/media/catalog/product/cache/1/image/1800x/040ec09b1e35df139433887a97daa66f/d/i/dievu_miskas.jpg" },
                    new Book { Name = "Madagaskaras", Year = 1844, Audiobook = true, PhysicalBook = false, ImageUrl = "https://thumb.knygos-static.lt/tL0BwYfeHodWrBIFhOnb-AjEQoM=/fit-in/0x800/filters:cwatermark(static/wm.png,500,75,30)/images/books/759479/1462885690_img082.jpg" },
                    new Book { Name = "Lazda", Year = 1814, Audiobook = true, PhysicalBook = true, ImageUrl = "https://thumb.knygos-static.lt/tuutrSc-ZCnV0UeIZ94QVChfn6Y=/fit-in/0x800/filters:cwatermark(static/wm.png,500,75,30)/images/books/1156220/1656484821_Scan0001.jpg" },
                });
                SaveChanges(); // Save changes to the database
            }
        }
    }
}
