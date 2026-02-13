using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TP.Models;
using TP.Services.Interfaces;

namespace TP.Controllers;

public class GenreController : Controller
{
    private readonly IGenreService _genreService;

    public GenreController(IGenreService genreService)
    {
        _genreService = genreService;
    }

    // GET: Genre
    public IActionResult Index()
    {
        return View(_genreService.GetAllGenres());
    }

    // GET: Genre/Details/5
    public IActionResult Details(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var genre = _genreService.GetGenreById(id.Value);
        if (genre == null)
        {
            return NotFound();
        }

        return View(genre);
    }

    // GET: Genre/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Genre/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Genre genre)
    {
        if (ModelState.IsValid)
        {
            _genreService.AddGenre(genre);
            return RedirectToAction(nameof(Index));
        }
        
        ViewBag.Errors = ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();
            
        return View(genre);
    }

    // GET: Genre/Edit/5
    public IActionResult Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var genre = _genreService.GetGenreById(id.Value);
        if (genre == null)
        {
            return NotFound();
        }
        return View(genre);
    }

    // POST: Genre/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Guid id, Genre genre)
    {
        if (id != genre.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _genreService.UpdateGenre(genre);
            }
            catch (Exception)
            {
                if (!_genreService.GenreExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        
        ViewBag.Errors = ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();
            
        return View(genre);
    }

    // GET: Genre/Delete/5
    public IActionResult Delete(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var genre = _genreService.GetGenreById(id.Value);
        if (genre == null)
        {
            return NotFound();
        }

        return View(genre);
    }

    // POST: Genre/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(Guid id)
    {
        _genreService.DeleteGenre(id);
        return RedirectToAction(nameof(Index));
    }
}