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
    private readonly IWebHostEnvironment _webHostEnvironment;

    public MovieController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
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
        ViewBag.Genres = new SelectList(_context.Genres, "Id", "Name");
        return View();
    }

    // POST: Movie/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(MovieVM model)
    {
        if (ModelState.IsValid)
        {
            if (model.Photo != null)
            {
                // Ensure wwwroot/images directory exists
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                Directory.CreateDirectory(uploadsFolder);

                // Generate unique filename
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Save file
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }

                model.Movie.ImageFile = uniqueFileName;
            }

            _context.Movies.Add(model.Movie);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // If we got this far, something failed, redisplay form
        ViewBag.Errors = ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

        ViewBag.Genres = new SelectList(_context.Genres, "Id", "Name", model.Movie.GenreId);
        return View(model);
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

        // Create MovieVM with existing movie data
        var movieVM = new MovieVM
        {
            Movie = movie
        };

        ViewBag.Genres = new SelectList(_context.Genres, "Id", "Name", movie.GenreId);
        return View(movieVM);
    }

    // POST: Movie/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, MovieVM model)
    {
        if (id != model.Movie.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var existingMovie = _context.Movies.Find(id);
                if (existingMovie == null)
                {
                    return NotFound();
                }

                // Handle photo upload if new one provided
                if (model.Photo != null)
                {
                    // Delete old image if exists
                    if (!string.IsNullOrEmpty(existingMovie.ImageFile))
                    {
                        string oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", existingMovie.ImageFile);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    // Save new image
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        model.Photo.CopyTo(fileStream);
                    }

                    existingMovie.ImageFile = uniqueFileName;
                }

                // Update other properties
                existingMovie.Name = model.Movie.Name;
                existingMovie.DateTimeMovie = model.Movie.DateTimeMovie;
                existingMovie.GenreId = model.Movie.GenreId;

                _context.Update(existingMovie);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Movies.Any(m => m.Id == id))
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

        ViewBag.Genres = new SelectList(_context.Genres, "Id", "Name", model.Movie.GenreId);
        return View(model);
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