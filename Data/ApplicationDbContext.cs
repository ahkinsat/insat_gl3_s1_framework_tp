using Microsoft.EntityFrameworkCore;
using TP.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;
using System.IO;

namespace TP.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Movie> Movies { get; set; } = default!;
    public DbSet<Genre> Genres { get; set; } = default!;
    public DbSet<Customer> Customers { get; set; } = default!;
    public DbSet<MembershipType> MembershipTypes { get; set; } = default!;
    public DbSet<AuditLog> AuditLogs { get; set; } = default!;
    public DbSet<UserCart> UserCarts { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Movie-Customer many-to-many relationship
        modelBuilder.Entity<Movie>()
            .HasMany(m => m.Customers)
            .WithMany(c => c.Movies)
            .UsingEntity(j => j.ToTable("MovieCustomers"));

        // Configure Customer-MembershipType relationship
        modelBuilder.Entity<Customer>()
            .HasOne(c => c.MembershipType)
            .WithMany(m => m.Customers)
            .HasForeignKey(c => c.MembershipTypeId);

        // Seed MembershipTypes for testing
        modelBuilder.Entity<MembershipType>().HasData(
            new MembershipType
            {
                Id = 1,
                Name = "Basic",
                DurationInMonths = 1,
                DiscountRate = 0,
                SignUpFee = 0
            },
            new MembershipType
            {
                Id = 2,
                Name = "Gold",
                DurationInMonths = 12,
                DiscountRate = 10,
                SignUpFee = 50
            },
            new MembershipType
            {
                Id = 3,
                Name = "Platinum",
                DurationInMonths = 24,
                DiscountRate = 20,
                SignUpFee = 100
            }
        );

        modelBuilder.Entity<UserCart>()
            .HasOne(uc => uc.User)
            .WithMany()
            .HasForeignKey(uc => uc.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserCart>()
            .HasOne(uc => uc.Movie)
            .WithMany()
            .HasForeignKey(uc => uc.MovieId)
            .OnDelete(DeleteBehavior.Cascade);

        // Seed Movies from JSON
        string movieJson = File.ReadAllText("Movies.json");
        var movies = System.Text.Json.JsonSerializer.Deserialize<List<Movie>>(movieJson);

        if (movies != null)
        {
            foreach (var movie in movies)
            {
                modelBuilder.Entity<Movie>().HasData(movie);
            }
        }
    }
}