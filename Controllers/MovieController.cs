using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TP.Models;
using TP.Data;
using X.PagedList;
using X.PagedList.Mvc.Core;
using TP.Services.Interfaces;

namespace TP.Controllers;

public class MovieController : Controller
{
    private readonly IMovieService _movieService;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ApplicationDbContext _context; // Still needed for genres dropdown

    public MovieController(IMovieService movieService, IWebHostEnvironment webHostEnvironment, ApplicationDbContext context)
    {
        _movieService = movieService;
        _webHostEnvironment = webHostEnvironment;
        _context = context;
    }

    // GET: Movie
    public IActionResult Index(string sortOrder, int? page)
    {
        ViewData["CurrentSort"] = sortOrder;
        ViewData["IdSortParm"] = string.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
        ViewData["NameSortParm"] = sortOrder == "name" ? "name_desc" : "name";
        
        var movies = _movieService.GetAllMovies().AsQueryable();
        
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
        
        return View(movies.ToPagedList(pageNumber, pageSize));
    }

    // GET: Movie/Details/5
    public IActionResult Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movie = _movieService.GetMovieById(id.Value);
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
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                Directory.CreateDirectory(uploadsFolder);
                
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }
                
                model.Movie.ImageFile = uniqueFileName;
            }
            
            _movieService.AddMovie(model.Movie);
            return RedirectToAction(nameof(Index));
        }
        
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

        var movie = _movieService.GetMovieById(id.Value);
        if (movie == null)
        {
            return NotFound();
        }
        
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
                var existingMovie = _movieService.GetMovieById(id);
                if (existingMovie == null)
                {
                    return NotFound();
                }
                
                // Handle photo upload
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
                
                _movieService.UpdateMovie(existingMovie);
            }
            catch (Exception)
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

    // GET: Movie/Delete/5
    public IActionResult Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movie = _movieService.GetMovieById(id.Value);
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
        _movieService.DeleteMovie(id);
        return RedirectToAction(nameof(Index));
    }

    // GET: Movie/CustomerMovies (from TP1)
    public IActionResult CustomerMovies()
    {
        var viewModel = new CustomerMovieViewModel
        {
            Customer = new Customer { Id = 1, Name = "John Doe" },
            Movies = _movieService.GetAllMovies().ToList()
        };
        return View(viewModel);
    }

    // GET: Movie/ByRelease (from TP1)
    public IActionResult ByRelease(int year, int month)
    {
        return Content($"Released in: {month}/{year}");
    }

    [Route("Movie/released/{year}/{month}")]
    public IActionResult ByReleaseAttribute(int year, int month)
    {
        return Content($"Released in (attribute): {month}/{year}");
    }
}