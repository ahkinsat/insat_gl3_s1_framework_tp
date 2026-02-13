using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TP.Models;
using TP.Data;
using TP.Services.Interfaces;

namespace TP.Controllers;

public class CustomerController : Controller
{
    private readonly ICustomerService _customerService;
    private readonly ApplicationDbContext _context; // Still needed for membership dropdown

    public CustomerController(ICustomerService customerService, ApplicationDbContext context)
    {
        _customerService = customerService;
        _context = context;
    }

    // GET: Customer
    public IActionResult Index()
    {
        return View(_customerService.GetAllCustomers());
    }

    // GET: Customer/Details/5
    public IActionResult Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var customer = _customerService.GetCustomerById(id.Value);
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
            _customerService.AddCustomer(customer);
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

        var customer = _customerService.GetCustomerById(id.Value);
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
                _customerService.UpdateCustomer(customer);
            }
            catch (Exception)
            {
                if (_customerService.GetCustomerById(id) == null)
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

        var customer = _customerService.GetCustomerById(id.Value);
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
        _customerService.DeleteCustomer(id);
        return RedirectToAction(nameof(Index));
    }
}