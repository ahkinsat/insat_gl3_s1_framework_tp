using Microsoft.AspNetCore.Mvc;
using TP.Models;

namespace TP.Controllers;

public class MovieController : Controller
{
    public IActionResult Index()
    {
        var movies = new List<Movie>
        {
            new Movie { Id = 1, Name = "Movie 1" },
            new Movie { Id = 2, Name = "Movie 2" },
            new Movie { Id = 3, Name = "Movie 3" },
            new Movie { Id = 4, Name = "Movie 4" }
        };
        return View(movies);
    }


    public IActionResult Details(int id)
    {
        var movies = new List<Movie>
        {
            new Movie { Id = 1, Name = "Movie 1" },
            new Movie { Id = 2, Name = "Movie 2" },
            new Movie { Id = 3, Name = "Movie 3" },
            new Movie { Id = 4, Name = "Movie 4" }
        };
        
        var movie = movies.FirstOrDefault(m => m.Id == id);
        
        if (movie == null)
        {
            return NotFound();
        }
        
        return View(movie);
    }

    public IActionResult Edit(int id)
    {
        return Content("Test Id: " + id);
    }

    // For exercise 6.1 - convention-based routing
    public IActionResult ByRelease(int year, int month)
    {
        return Content($"Released in: {month}/{year}");
    }

    // For exercise 6.1 - attribute-based routing
    [Route("Movie/released/{year}/{month}")]
    public IActionResult ByReleaseAttribute(int year, int month)
    {
        return Content($"Released in (attribute): {month}/{year}");
    }

    // For exercise 6.2 - ViewModel
    public IActionResult CustomerMovies()
    {
        var viewModel = new CustomerMovieViewModel
        {
            Customer = new Customer { Id = 1, Name = "John Doe" },
            Movies = new List<Movie>
            {
                new Movie { Id = 1, Name = "Movie 1" },
                new Movie { Id = 2, Name = "Movie 2" },
                new Movie { Id = 3, Name = "Movie 3" }
            }
        };
        return View(viewModel);
    }
}