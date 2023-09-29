using ProvaPub.Models;

namespace ProvaPub.Services
{
    public interface ICustomerService
    {
        public CustomerList ListCustomers(int page);
        public Task<bool> CanPurchase(int customerId, decimal purchaseValue);
    }
}
