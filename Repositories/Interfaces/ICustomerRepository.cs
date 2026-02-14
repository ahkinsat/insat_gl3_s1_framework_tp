using TP.Models;

namespace TP.Repositories.Interfaces;

public interface ICustomerRepository : IGenericRepository<Customer>
{
    Customer? GetCustomerWithMembershipAndMovies(int id);
    IEnumerable<Customer> GetSubscribedCustomersWithHighDiscount(decimal discountThreshold);
}