using Microsoft.EntityFrameworkCore;
using TP.Data;
using TP.Models;
using TP.Services.Interfaces;

namespace TP.Services;

public class MovieService : IMovieService
{
    private readonly ApplicationDbContext _context;

    public MovieService(ApplicationDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Movie> GetAllMovies()
    {
        return _context.Movies
            .Include(m => m.Genre)
            .ToList();
    }

    public Movie? GetMovieById(int id)
    {
        return _context.Movies
            .Include(m => m.Genre)
            .FirstOrDefault(m => m.Id == id);
    }

    public void AddMovie(Movie movie)
    {
        _context.Movies.Add(movie);
        _context.SaveChanges();
    }

    public void UpdateMovie(Movie movie)
    {
        _context.Movies.Update(movie);
        _context.SaveChanges();
    }

    public void DeleteMovie(int id)
    {
        var movie = _context.Movies.Find(id);
        if (movie != null)
        {
            _context.Movies.Remove(movie);
            _context.SaveChanges();
        }
    }

    public IEnumerable<Movie> GetActionMoviesInStock()
    {
        return _context.Movies
            .Include(m => m.Genre)
            .Where(m => m.Genre != null && m.Genre.Name == "Action" && m.Stock > 0)
            .ToList();
    }

    public IEnumerable<Movie> GetMoviesOrderedByDateThenName()
    {
        return _context.Movies
            .OrderBy(m => m.DateTimeMovie)
            .ThenBy(m => m.Name)
            .ToList();
    }

    public int GetTotalMovieCount()
    {
        return _context.Movies.Count();
    }

    public IEnumerable<object> GetMoviesWithGenres()
    {
        return _context.Movies
            .Join(_context.Genres,
                movie => movie.GenreId,
                genre => genre.Id,
                (movie, genre) => new
                {
                    MovieTitle = movie.Name,
                    GenreName = genre.Name
                })
            .ToList();
    }
}