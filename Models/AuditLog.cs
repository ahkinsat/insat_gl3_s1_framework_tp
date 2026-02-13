namespace TP.Models;

public class AuditLog
{
    public int Id { get; set; }
    public string TableName { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string EntityKey { get; set; } = string.Empty;
    public string? Changes { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
}