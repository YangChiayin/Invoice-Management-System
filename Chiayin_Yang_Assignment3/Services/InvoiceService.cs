using Chiayin_Yang_Assignment3.Data;
using Invoicing.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chiayin_Yang_Assignment3.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly InvoicesDbContext _invoicesDbContext;

        public InvoiceService(InvoicesDbContext invoicesDbContext)
        {
            _invoicesDbContext = invoicesDbContext;
        }

        // Fetches the customer along with their invoices, line items, and payment terms
        public async Task<Customer> GetCustomerWithInvoicesAsync(int customerId)
        {
            var customer = await _invoicesDbContext.Customers
                .Include(c => c.Invoices) // Include Invoices for the customer
                    .ThenInclude(i => i.InvoiceLineItems)  // Include line items for each invoice
                .Include(c => c.Invoices)
                    .ThenInclude(i => i.PaymentTerms) // Include payment terms for each invoice
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);

            return customer;
        }

        // Fetches the list of invoices for a specific customer
        public async Task<List<Invoice>> GetInvoicesForCustomerAsync(int customerId)
        {
            return await _invoicesDbContext.Invoices
                .Where(i => i.CustomerId == customerId) // Filter by customer ID
                .OrderBy(i => i.InvoiceDate) // Order by invoice date
                .ToListAsync(); // Return as a list
        }
    }
}