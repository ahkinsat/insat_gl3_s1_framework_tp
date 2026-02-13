using TP.Models;

namespace TP.Services.Interfaces;

public interface IGenreService
{
    IEnumerable<Genre> GetAllGenres();
    Genre? GetGenreById(Guid id);
    void AddGenre(Genre genre);
    void UpdateGenre(Genre genre);
    void DeleteGenre(Guid id);
    bool GenreExists(Guid id);
    IEnumerable<Genre> GetTop3PopularGenres();
}