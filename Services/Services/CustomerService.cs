using Microsoft.EntityFrameworkCore;
using TP.Data;
using TP.Models;
using TP.Services.Interfaces;

namespace TP.Services;

public class CustomerService : ICustomerService
{
    private readonly ApplicationDbContext _context;

    public CustomerService(ApplicationDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Customer> GetAllCustomers()
    {
        return _context.Customers
            .Include(c => c.MembershipType)
            .Include(c => c.Movies)
            .ToList();
    }

    public Customer? GetCustomerById(int id)
    {
        return _context.Customers
            .Include(c => c.MembershipType)
            .Include(c => c.Movies)
            .FirstOrDefault(c => c.Id == id);
    }

    public void AddCustomer(Customer customer)
    {
        _context.Customers.Add(customer);
        _context.SaveChanges();
    }

    public void UpdateCustomer(Customer customer)
    {
        _context.Customers.Update(customer);
        _context.SaveChanges();
    }

    public void DeleteCustomer(int id)
    {
        var customer = _context.Customers
            .Include(c => c.Movies)
            .FirstOrDefault(c => c.Id == id);

        if (customer != null)
        {
            _context.Customers.Remove(customer);
            _context.SaveChanges();
        }
    }

    public IEnumerable<Movie> GetCustomerMovies(int customerId)
    {
        var customer = _context.Customers
            .Include(c => c.Movies)
            .FirstOrDefault(c => c.Id == customerId);

        return customer?.Movies ?? new List<Movie>();
    }
    
    public IEnumerable<Customer> GetSubscribedCustomersWithHighDiscount(decimal discountThreshold)
    {
        return _context.Customers
            .Include(c => c.MembershipType)
            .Where(c => c.IsSubscribed &&
                        c.MembershipType != null &&
                        c.MembershipType.DiscountRate > discountThreshold)
            .ToList();
    }
}