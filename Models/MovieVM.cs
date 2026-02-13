namespace TP.Models;

public class MovieVM
{
    public Movie Movie { get; set; } = new();
    public IFormFile? Photo { get; set; }
}