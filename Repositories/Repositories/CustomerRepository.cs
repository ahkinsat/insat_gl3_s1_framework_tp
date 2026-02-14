using Microsoft.EntityFrameworkCore;
using TP.Data;
using TP.Models;
using TP.Repositories.Interfaces;

namespace TP.Repositories;

public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(ApplicationDbContext context) : base(context)
    {
    }

    public Customer? GetCustomerWithMembershipAndMovies(int id)
    {
        return _dbSet
            .Include(c => c.MembershipType)
            .Include(c => c.Movies)
            .FirstOrDefault(c => c.Id == id);
    }

    public IEnumerable<Customer> GetSubscribedCustomersWithHighDiscount(decimal discountThreshold)
    {
        return _dbSet
            .Include(c => c.MembershipType)
            .Where(c => c.IsSubscribed && 
                        c.MembershipType != null && 
                        c.MembershipType.DiscountRate > discountThreshold)
            .ToList();
    }
}