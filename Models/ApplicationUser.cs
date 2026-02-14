using Microsoft.AspNetCore.Identity;

namespace TP.Models;

public class ApplicationUser : IdentityUser
{
    public string? City { get; set; }
}