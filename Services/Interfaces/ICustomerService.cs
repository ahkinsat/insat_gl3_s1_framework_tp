using TP.Models;

namespace TP.Services.Interfaces;

public interface ICustomerService
{
    IEnumerable<Customer> GetAllCustomers();
    Customer? GetCustomerById(int id);
    void AddCustomer(Customer customer);
    void UpdateCustomer(Customer customer);
    void DeleteCustomer(int id);
    IEnumerable<Movie> GetCustomerMovies(int customerId);
    IEnumerable<Customer> GetSubscribedCustomersWithHighDiscount(decimal discountThreshold);
}