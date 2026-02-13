using Microsoft.EntityFrameworkCore;
using TP.Data;
using TP.Models;
using TP.Services.Interfaces;

namespace TP.Services;

public class GenreService : IGenreService
{
    private readonly ApplicationDbContext _context;

    public GenreService(ApplicationDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Genre> GetAllGenres()
    {
        return _context.Genres
            .Include(g => g.Movies)
            .ToList();
    }

    public Genre? GetGenreById(Guid id)
    {
        return _context.Genres
            .Include(g => g.Movies)
            .FirstOrDefault(g => g.Id == id);
    }

    public void AddGenre(Genre genre)
    {
        genre.Id = Guid.NewGuid();
        _context.Genres.Add(genre);
        _context.SaveChanges();
    }

    public void UpdateGenre(Genre genre)
    {
        _context.Genres.Update(genre);
        _context.SaveChanges();
    }

    public void DeleteGenre(Guid id)
    {
        var genre = _context.Genres
            .Include(g => g.Movies)
            .FirstOrDefault(g => g.Id == id);

        if (genre != null)
        {
            // Optional: Check if genre has movies before deleting
            _context.Genres.Remove(genre);
            _context.SaveChanges();
        }
    }

    public bool GenreExists(Guid id)
    {
        return _context.Genres.Any(g => g.Id == id);
    }

    public IEnumerable<Genre> GetTop3PopularGenres()
    {
        return _context.Genres
            .Include(g => g.Movies)
            .Select(g => new { Genre = g, MovieCount = _context.Movies.Count(m => m.GenreId == g.Id) })
            .OrderByDescending(x => x.MovieCount)
            .Take(3)
            .Select(x => x.Genre)
            .ToList();
    }
}