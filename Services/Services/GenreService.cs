using TP.Models;
using TP.Repositories.Interfaces;
using TP.Services.Interfaces;

namespace TP.Services;

public class GenreService : IGenreService
{
    private readonly IGenreRepository _genreRepository;
    private readonly IMovieRepository _movieRepository;


    public GenreService(IGenreRepository genreRepository, IMovieRepository movieRepository)
    {
        _genreRepository = genreRepository;
        _movieRepository = movieRepository;
    }

    public IEnumerable<Genre> GetAllGenres()
    {
        return _genreRepository.GetAll();
    }

    public Genre? GetGenreById(Guid id)
    {
        return _genreRepository.GetGenreWithMovies(id);
    }

    public void AddGenre(Genre genre)
    {
        genre.Id = Guid.NewGuid();
        _genreRepository.Add(genre);
        _genreRepository.SaveChanges();
    }

    public void UpdateGenre(Genre genre)
    {
        _genreRepository.Update(genre);
        _genreRepository.SaveChanges();
    }

    public void DeleteGenre(Guid id)
    {
        var genre = _genreRepository.GetById(id);
        if (genre != null)
        {
            _genreRepository.Delete(genre);
            _genreRepository.SaveChanges();
        }
    }

    public bool GenreExists(Guid id)
    {
        return _genreRepository.GetById(id) != null;
    }

    public IEnumerable<Genre> GetTop3PopularGenres()
    {
        return _movieRepository.GetTop3PopularGenres(); // We need to inject IMovieRepository
    }
}