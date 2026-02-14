using TP.Models;
using TP.Repositories.Interfaces;
using TP.Services.Interfaces;

namespace TP.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;

    public MovieService(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    public IEnumerable<Movie> GetAllMovies()
    {
        return _movieRepository.GetMoviesWithGenre();
    }

    public Movie? GetMovieById(int id)
    {
        return _movieRepository.GetById(id);
    }

    public void AddMovie(Movie movie)
    {
        _movieRepository.Add(movie);
        _movieRepository.SaveChanges();
    }

    public void UpdateMovie(Movie movie)
    {
        _movieRepository.Update(movie);
        _movieRepository.SaveChanges();
    }

    public void DeleteMovie(int id)
    {
        var movie = _movieRepository.GetById(id);
        if (movie != null)
        {
            _movieRepository.Delete(movie);
            _movieRepository.SaveChanges();
        }
    }

    // LINQ methods
    public IEnumerable<Movie> GetActionMoviesInStock()
    {
        return _movieRepository.GetActionMoviesInStock();
    }

    public IEnumerable<Movie> GetMoviesOrderedByDateThenName()
    {
        return _movieRepository.GetMoviesOrderedByDateThenName();
    }

    public int GetTotalMovieCount()
    {
        return _movieRepository.GetAll().Count();
    }

    public IEnumerable<object> GetMoviesWithGenres()
    {
        return _movieRepository.GetMoviesWithGenresJoin();
    }
}