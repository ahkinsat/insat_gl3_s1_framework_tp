using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TP.Models;
using TP.Data;
using X.PagedList;
using X.PagedList.Mvc.Core;

namespace TP.Controllers;

public class MovieController : Controller
{
    private readonly ApplicationDbContext _context;

    public MovieController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string sortOrder, int? page)
    {
        ViewData["CurrentSort"] = sortOrder;
        ViewData["IdSortParm"] = string.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
        ViewData["NameSortParm"] = sortOrder == "name" ? "name_desc" : "name";

        var movies = from m in _context.Movies.Include(m => m.Genre)
                     select m;

        // Sorting
        switch (sortOrder)
        {
            case "id_desc":
                movies = movies.OrderByDescending(m => m.Id);
                break;
            case "name":
                movies = movies.OrderBy(m => m.Name);
                break;
            case "name_desc":
                movies = movies.OrderByDescending(m => m.Name);
                break;
            default:
                movies = movies.OrderBy(m => m.Id);
                break;
        }

        // Pagination
        int pageSize = 3;
        int pageNumber = (page ?? 1);

        return View(await movies.ToPagedListAsync(pageNumber, pageSize));
    }

    // GET: Movie/Details/5
    public IActionResult Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movie = _context.Movies
            .Include(m => m.Genre)  // Include the genre
            .FirstOrDefault(m => m.Id == id);

        if (movie == null)
        {
            return NotFound();
        }

        return View(movie);
    }

    // GET: Movie/Create
    public IActionResult Create()
    {
        ViewData["Genres"] = new SelectList(_context.Genres, "Id", "Name");
        return View();
    }

    // POST: Movie/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Movie movie)
    {
        if (ModelState.IsValid)
        {
            _context.Add(movie);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        return View(movie);
    }
    // GET: Movie/Edit/5
    public IActionResult Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movie = _context.Movies.Find(id);
        if (movie == null)
        {
            return NotFound();
        }

        // Pass genres to view
        ViewData["Genres"] = new SelectList(_context.Genres, "Id", "Name", movie.GenreId);
        return View(movie);
    }

    // POST: Movie/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Movie movie)
    {
        if (id != movie.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            _context.Update(movie);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // If model state invalid, reload genres and return view
        ViewData["Genres"] = new SelectList(_context.Genres, "Id", "Name", movie.GenreId);
        return View(movie);
    }

    public IActionResult ByRelease(int year, int month)
    {
        return Content($"Released in: {month}/{year}");
    }

    [Route("Movie/released/{year}/{month}")]
    public IActionResult ByReleaseAttribute(int year, int month)
    {
        return Content($"Released in (attribute): {month}/{year}");
    }

    public IActionResult CustomerMovies()
    {
        var viewModel = new CustomerMovieViewModel
        {
            Customer = new Customer { Id = 1, Name = "John Doe" },
            Movies = _context.Movies.ToList()
        };
        return View(viewModel);
    }

    // GET: Movie/Delete/5
    public IActionResult Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movie = _context.Movies.FirstOrDefault(m => m.Id == id);
        if (movie == null)
        {
            return NotFound();
        }
        return View(movie);
    }

    // POST: Movie/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var movie = _context.Movies.Find(id);
        if (movie != null)
        {
            _context.Movies.Remove(movie);
            _context.SaveChanges();
        }
        return RedirectToAction(nameof(Index));
    }
}