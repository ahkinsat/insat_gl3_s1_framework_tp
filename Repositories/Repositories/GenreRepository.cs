using Microsoft.EntityFrameworkCore;
using TP.Data;
using TP.Models;
using TP.Repositories.Interfaces;

namespace TP.Repositories;

public class GenreRepository : GenericRepository<Genre>, IGenreRepository
{
    public GenreRepository(ApplicationDbContext context) : base(context)
    {
    }

    public Genre? GetGenreWithMovies(Guid id)
    {
        return _dbSet
            .Include(g => g.Movies)
            .FirstOrDefault(g => g.Id == id);
    }

    public IEnumerable<Genre> GetGenresWithMovieCount()
    {
        return _dbSet
            .Include(g => g.Movies)
            .Select(g => new Genre
            {
                Id = g.Id,
                Name = g.Name,
                Movies = g.Movies
            })
            .ToList();
    }
}