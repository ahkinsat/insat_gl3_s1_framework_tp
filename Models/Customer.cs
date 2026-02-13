namespace TP.Models;

public class Customer
{
    public int Id { get; set; }
    public string? Name { get; set; } = String.Empty;
    
    // Navigation properties for relationships-
    public int? MembershipTypeId { get; set; }
    public MembershipType? MembershipType { get; set; }
    
    // Many-to-Many with Movies
    public ICollection<Movie>? Movies { get; set; }
}