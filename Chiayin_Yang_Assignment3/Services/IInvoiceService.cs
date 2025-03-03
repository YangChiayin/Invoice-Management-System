using Invoicing.Entities;

namespace Chiayin_Yang_Assignment3.Services
{
    public interface IInvoiceService
    {
        Task<Customer> GetCustomerWithInvoicesAsync(int customerId);
        Task<List<Invoice>> GetInvoicesForCustomerAsync(int customerId);
    }
}
