using Microsoft.AspNetCore.Mvc;
using TP.Services.Interfaces;

namespace TP.Controllers;

public class TestLinqController : Controller
{
    private readonly IMovieService _movieService;
    private readonly ICustomerService _customerService;
    private readonly IGenreService _genreService;

    public TestLinqController(
        IMovieService movieService, 
        ICustomerService customerService, 
        IGenreService genreService)
    {
        _movieService = movieService;
        _customerService = customerService;
        _genreService = genreService;
    }

    public IActionResult Index()
    {
        ViewBag.ActionMovies = _movieService.GetActionMoviesInStock();
        ViewBag.OrderedMovies = _movieService.GetMoviesOrderedByDateThenName();
        ViewBag.TotalMovies = _movieService.GetTotalMovieCount();
        ViewBag.SubscribedCustomers = _customerService.GetSubscribedCustomersWithHighDiscount(10);
        ViewBag.MoviesWithGenres = _movieService.GetMoviesWithGenres();
        ViewBag.TopGenres = _genreService.GetTop3PopularGenres();
        
        return View();
    }
}