using System.ComponentModel.DataAnnotations.Schema;

namespace TP.Models;

public class UserCart
{
    public Guid Id { get; set; }
    
    // Foreign keys
    public string UserId { get; set; } = string.Empty;
    public int MovieId { get; set; }
    
    // Tracking
    public DateTime AddedDate { get; set; } = DateTime.UtcNow;
    public int Quantity { get; set; } = 1;
    
    // Navigation properties
    [ForeignKey("UserId")]
    public ApplicationUser? User { get; set; }
    
    [ForeignKey("MovieId")]
    public Movie? Movie { get; set; }
}