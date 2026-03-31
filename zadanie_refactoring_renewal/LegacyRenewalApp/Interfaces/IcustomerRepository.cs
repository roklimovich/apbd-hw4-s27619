namespace LegaceRenewalApp.Interfaces
{
    public interface ICustomerRepository
    {
        Customer GetById(int customerId);
    }
}