using LegacyRenewalApp.Interfaces;
 
namespace LegacyRenewalApp.Infrastructure
{
    public class CustomerRepositoryAdapter : ICustomerRepository
    {
        private readonly CustomerRepository _inner = new CustomerRepository();
 
        public Customer GetById(int customerId) => _inner.GetById(customerId);
    }
}