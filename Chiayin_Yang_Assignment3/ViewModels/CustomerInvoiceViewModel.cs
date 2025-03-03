using Invoicing.Entities;

namespace Chiayin_Yang_Assignment3.ViewModels
{
    public class CustomerInvoiceViewModel
    {
        public Customer? Customer { get; set; } // Customer details
        public Invoice? ActiveInvoice { get; set; } // Currently focused Invoice
        public List<Invoice>? Invoices { get; set; } // List of all invoices for the customer
        public List<PaymentTerms>? PaymentTerms { get; set; } // List of PaymentTerms for dropdown

 //       public List<InvoiceLineItem>? InvoicesLineItems { get; set; }

    }
}
