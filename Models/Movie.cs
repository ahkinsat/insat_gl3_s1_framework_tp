namespace TP.Models;

public class Movie
{
    public int Id { get; set; }
    public string? Name { get; set; }
    
    public Guid? GenreId { get; set; }
    public Genre? Genre { get; set; }
    
    public string? ImageFile { get; set; }
    public DateTime? DateTimeMovie { get; set; }

    public int Stock { get; set; }
    
    // Many-to-Many with Customers
    public ICollection<Customer>? Customers { get; set; }
}