using Microsoft.EntityFrameworkCore;
using TP.Data;
using TP.Models;
using TP.Repositories.Interfaces;

namespace TP.Repositories;

public class MovieRepository : GenericRepository<Movie>, IMovieRepository
{
    public MovieRepository(ApplicationDbContext context) : base(context)
    {
    }

    public IEnumerable<Movie> GetMoviesWithGenre()
    {
        return _dbSet
            .Include(m => m.Genre)
            .ToList();
    }

    public IEnumerable<Movie> GetActionMoviesInStock()
    {
        return _dbSet
            .Include(m => m.Genre)
            .Where(m => m.Genre != null && m.Genre.Name == "Action" && m.Stock > 0)
            .ToList();
    }

    public IEnumerable<Movie> GetMoviesOrderedByDateThenName()
    {
        return _dbSet
            .OrderBy(m => m.DateTimeMovie)
            .ThenBy(m => m.Name)
            .ToList();
    }

    public IEnumerable<object> GetMoviesWithGenresJoin()
    {
        return _dbSet
            .Join(_context.Genres,
                movie => movie.GenreId,
                genre => genre.Id,
                (movie, genre) => new { 
                    MovieTitle = movie.Name, 
                    GenreName = genre.Name 
                })
            .ToList();
    }

    public IEnumerable<Genre> GetTop3PopularGenres()
    {
        return _context.Genres
            .Include(g => g.Movies)
            .Select(g => new { Genre = g, MovieCount = g.Movies != null ? g.Movies.Count : 0 })
            .OrderByDescending(x => x.MovieCount)
            .Take(3)
            .Select(x => x.Genre)
            .ToList();
    }
}