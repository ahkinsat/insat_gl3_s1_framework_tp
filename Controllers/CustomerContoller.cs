using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TP.Data;
using TP.Models;

namespace TP.Controllers;

public class CustomerController : Controller
{
    private readonly ApplicationDbContext _context;

    public CustomerController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Customer
    public IActionResult Index()
    {
        var customers = _context.Customers
            .Include(c => c.MembershipType)
            .ToList();
        return View(customers);
    }

    // GET: Customer/Details/5
    public IActionResult Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var customer = _context.Customers
            .Include(c => c.MembershipType)
            .Include(c => c.Movies)
            .FirstOrDefault(c => c.Id == id);
            
        if (customer == null)
        {
            return NotFound();
        }

        return View(customer);
    }

    // GET: Customer/Create
    public IActionResult Create()
    {
        ViewBag.MembershipTypes = new SelectList(_context.MembershipTypes, "Id", "Name");
        return View();
    }

    // POST: Customer/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Customer customer)
    {
        if (ModelState.IsValid)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        
        ViewBag.Errors = ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();
        
        ViewBag.MembershipTypes = new SelectList(_context.MembershipTypes, "Id", "Name", customer.MembershipTypeId);
        return View(customer);
    }

    // GET: Customer/Edit/5
    public IActionResult Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var customer = _context.Customers.Find(id);
        if (customer == null)
        {
            return NotFound();
        }
        
        ViewBag.MembershipTypes = new SelectList(_context.MembershipTypes, "Id", "Name", customer.MembershipTypeId);
        return View(customer);
    }

    // POST: Customer/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Customer customer)
    {
        if (id != customer.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(customer);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Customers.Any(c => c.Id == id))
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
        
        ViewBag.MembershipTypes = new SelectList(_context.MembershipTypes, "Id", "Name", customer.MembershipTypeId);
        return View(customer);
    }

    // GET: Customer/Delete/5
    public IActionResult Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var customer = _context.Customers
            .Include(c => c.MembershipType)
            .FirstOrDefault(c => c.Id == id);
            
        if (customer == null)
        {
            return NotFound();
        }

        return View(customer);
    }

    // POST: Customer/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var customer = _context.Customers.Find(id);
        if (customer != null)
        {
            _context.Customers.Remove(customer);
            _context.SaveChanges();
        }
        return RedirectToAction(nameof(Index));
    }
}