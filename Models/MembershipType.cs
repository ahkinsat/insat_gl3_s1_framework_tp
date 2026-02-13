namespace TP.Models;

public class MembershipType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DurationInMonths { get; set; }
    public decimal DiscountRate { get; set; }
    public decimal SignUpFee { get; set; }
    
    // Navigation property
    public ICollection<Customer>? Customers { get; set; }
}