using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TP.Models;
using TP.Data;

namespace TP.Controllers;

public class GenreController : Controller
{
    private readonly ApplicationDbContext _context;

    public GenreController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Genre
    public async Task<IActionResult> Index()
    {
        return View(await _context.Genres.ToListAsync());
    }


    // GET: Genre/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var genre = await _context.Genres
            .Include(g => g.Movies)  // Include related movies
            .FirstOrDefaultAsync(m => m.Id == id);

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
    public async Task<IActionResult> Create([Bind("Id,Name")] Genre genre)
    {
        if (ModelState.IsValid)
        {
            genre.Id = Guid.NewGuid();
            _context.Add(genre);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(genre);
    }

    // GET: Genre/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var genre = await _context.Genres.FindAsync(id);
        if (genre == null)
        {
            return NotFound();
        }
        return View(genre);
    }

    // POST: Genre/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name")] Genre genre)
    {
        if (id != genre.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(genre);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GenreExists(genre.Id))
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
        return View(genre);
    }

    // GET: Genre/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var genre = await _context.Genres
            .Include(g => g.Movies)  // Include to check if has movies
            .FirstOrDefaultAsync(m => m.Id == id);

        if (genre == null)
        {
            return NotFound();
        }

        return View(genre);
    }

    // POST: Genre/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var genre = await _context.Genres
            .Include(g => g.Movies)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (genre != null)
        {
            // Optional: Check if genre has movies before deleting
            if (genre.Movies != null && genre.Movies.Any())
            {
                TempData["Error"] = "Cannot delete genre with existing movies";
                return RedirectToAction(nameof(Index));
            }

            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private bool GenreExists(Guid id)
    {
        return _context.Genres.Any(e => e.Id == id);
    }
}