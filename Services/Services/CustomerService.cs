using TP.Models;
using TP.Repositories.Interfaces;
using TP.Services.Interfaces;

namespace TP.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public IEnumerable<Customer> GetAllCustomers()
    {
        return _customerRepository.GetAll();
    }

    public Customer? GetCustomerById(int id)
    {
        return _customerRepository.GetCustomerWithMembershipAndMovies(id);
    }

    public void AddCustomer(Customer customer)
    {
        _customerRepository.Add(customer);
        _customerRepository.SaveChanges();
    }

    public void UpdateCustomer(Customer customer)
    {
        _customerRepository.Update(customer);
        _customerRepository.SaveChanges();
    }

    public void DeleteCustomer(int id)
    {
        var customer = _customerRepository.GetById(id);
        if (customer != null)
        {
            _customerRepository.Delete(customer);
            _customerRepository.SaveChanges();
        }
    }

    public IEnumerable<Movie> GetCustomerMovies(int customerId)
    {
        var customer = _customerRepository.GetCustomerWithMembershipAndMovies(customerId);
        return customer?.Movies ?? new List<Movie>();
    }

    public IEnumerable<Customer> GetSubscribedCustomersWithHighDiscount(decimal discountThreshold)
    {
        return _customerRepository.GetSubscribedCustomersWithHighDiscount(discountThreshold);
    }
}