using Chiayin_Yang_Assignment3.Data;
using Chiayin_Yang_Assignment3.Services;
using Chiayin_Yang_Assignment3.ViewModels;
using Invoicing.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Chiayin_Yang_Assignment3.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly IInvoiceService _invoiceService;
        private readonly InvoicesDbContext _invoicesDbContext;

        public InvoiceController(IInvoiceService invoiceService, InvoicesDbContext invoicesDbContext)
        {
            _invoiceService = invoiceService;
            _invoicesDbContext = invoicesDbContext;
        }

        public async Task<IActionResult> Index(int customerId, int? selectedInvoiceId = null)
        {
            var customer = await _invoiceService.GetCustomerWithInvoicesAsync(customerId);
            if (customer == null)
            {
                return NotFound();
            }

            var selectedInvoice = selectedInvoiceId.HasValue
                ? customer.Invoices.FirstOrDefault(i => i.InvoiceId == selectedInvoiceId.Value)
                : null;

            var viewModel = new CustomerInvoiceViewModel
            {
                Customer = customer,
                // Change to OrderBy to sort in ascending order
                Invoices = customer.Invoices.OrderBy(i => i.InvoiceId).ToList(),
                ActiveInvoice = selectedInvoice ?? new Invoice(),
                PaymentTerms = await _invoicesDbContext.PaymentTerms.ToListAsync()
            };

            if (viewModel.ActiveInvoice.InvoiceLineItems == null)
            {
                viewModel.ActiveInvoice.InvoiceLineItems = new List<InvoiceLineItem>();
            }

            return View(viewModel);
        }

        // POST: Add an invoice
        [HttpPost]
        public async Task<IActionResult> AddInvoice(int customerId, Invoice invoice)
        {
            try
            {
                invoice.CustomerId = customerId;

                if (invoice.InvoiceLineItems != null)
                {
                    foreach (var item in invoice.InvoiceLineItems)
                    {
                        item.InvoiceId = 0;
                        item.InvoiceLineItemId = 0;
                    }
                }

                _invoicesDbContext.Invoices.Add(invoice);
                await _invoicesDbContext.SaveChangesAsync();

                var customer = await _invoiceService.GetCustomerWithInvoicesAsync(customerId);
                var viewModel = new CustomerInvoiceViewModel
                {
                    Customer = customer,
                    Invoices = customer.Invoices.OrderBy(i => i.InvoiceId).ToList(),
                    ActiveInvoice = null, // Change this line to null
                    PaymentTerms = await _invoicesDbContext.PaymentTerms.ToListAsync()
                };

                return PartialView("_InvoiceTablePartial", viewModel);
            }
            catch (Exception ex)
            {
                return BadRequest("There was an error adding the invoice.");
            }
        }

        // POST: Add a line item to the invoice
        [HttpPost]
        public async Task<IActionResult> AddLineItem(int invoiceId, string description, double amount)
        {
            var invoice = await _invoicesDbContext.Invoices
                .Include(i => i.InvoiceLineItems)
                .FirstOrDefaultAsync(i => i.InvoiceId == invoiceId);

            if (invoice != null)
            {
                var lineItem = new InvoiceLineItem
                {
                    InvoiceId = invoiceId,
                    Description = description,
                    Amount = amount
                };

                _invoicesDbContext.InvoicesLineItems.Add(lineItem);
                await _invoicesDbContext.SaveChangesAsync();
            }

            // Reload the invoice and its line items
            var updatedInvoice = await _invoicesDbContext.Invoices
                .Include(i => i.InvoiceLineItems)
                .FirstOrDefaultAsync(i => i.InvoiceId == invoiceId);

            // Return the updated table as a partial view
            return PartialView("_LineItemsTablePartial", updatedInvoice);
        }
    }
}
