using TP.Models;

namespace TP.Services.Interfaces;

public interface IMovieService
{
    IEnumerable<Movie> GetAllMovies();
    Movie? GetMovieById(int id);
    void AddMovie(Movie movie);
    void UpdateMovie(Movie movie);
    void DeleteMovie(int id);

    // LINQ methods
    IEnumerable<Movie> GetActionMoviesInStock();
    IEnumerable<Movie> GetMoviesOrderedByDateThenName();
    int GetTotalMovieCount();
    IEnumerable<object> GetMoviesWithGenres();
}