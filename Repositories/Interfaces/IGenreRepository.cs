using TP.Models;

namespace TP.Repositories.Interfaces;

public interface IGenreRepository : IGenericRepository<Genre>
{
    Genre? GetGenreWithMovies(Guid id);
    IEnumerable<Genre> GetGenresWithMovieCount();
}