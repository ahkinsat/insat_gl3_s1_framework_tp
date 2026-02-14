using TP.Models;

namespace TP.Repositories.Interfaces;

public interface IMovieRepository : IGenericRepository<Movie>
{
    IEnumerable<Movie> GetMoviesWithGenre();
    IEnumerable<Movie> GetActionMoviesInStock();
    IEnumerable<Movie> GetMoviesOrderedByDateThenName();
    IEnumerable<object> GetMoviesWithGenresJoin();
    IEnumerable<Genre> GetTop3PopularGenres();
}