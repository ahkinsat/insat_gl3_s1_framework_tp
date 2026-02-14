using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TP.Data;
using TP.Models;

namespace TP.Controllers;

[Authorize]
public class CartController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public CartController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: Cart/MyCart
    public async Task<IActionResult> MyCart()
    {
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser == null)
        {
            return Challenge();
        }

        var cartItems = await _context.UserCarts
            .Include(c => c.Movie)
            .Where(c => c.UserId == currentUser.Id)
            .ToListAsync();

        return View(cartItems);
    }

    // GET: Cart/AddToCart/5
    public async Task<IActionResult> AddToCart(int movieId)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser == null)
        {
            return Challenge();
        }

        var movie = await _context.Movies.FindAsync(movieId);
        if (movie == null)
        {
            return NotFound();
        }

        // Check if already in cart
        var existingItem = await _context.UserCarts
            .FirstOrDefaultAsync(c => c.UserId == currentUser.Id && c.MovieId == movieId);

        if (existingItem != null)
        {
            existingItem.Quantity++;
        }
        else
        {
            var cartItem = new UserCart
            {
                Id = Guid.NewGuid(),
                UserId = currentUser.Id,
                MovieId = movieId,
                AddedDate = DateTime.UtcNow,
                Quantity = 1
            };
            _context.UserCarts.Add(cartItem);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(MyCart));
    }

    // POST: Cart/RemoveFromCart/5
    [HttpPost]
    public async Task<IActionResult> RemoveFromCart(Guid id)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser == null)
        {
            return Challenge();
        }

        var cartItem = await _context.UserCarts
            .FirstOrDefaultAsync(c => c.Id == id && c.UserId == currentUser.Id);

        if (cartItem != null)
        {
            _context.UserCarts.Remove(cartItem);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(MyCart));
    }
}