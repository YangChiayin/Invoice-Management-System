namespace Invoicing.Entities
{
    public class Invoice
    {
        public int InvoiceId { get; set; }

        public DateTime? InvoiceDate { get; set; }

        public DateTime? InvoiceDueDate
        {
            get
            {
                return InvoiceDate?.AddDays(Convert.ToDouble(PaymentTerms?.DueDays));
            }
        }

        public double? PaymentTotal { get; set; } = 0.0;

        public DateTime? PaymentDate { get; set; }

        // FK:
        public int PaymentTermsId { get; set; }

        // FK:
        public int CustomerId { get; set; }


        // Navigation property: An Invoice belongs to a Customer
        public Customer Customer { get; set; } = null!;
        // Navigation property: An Invoice can have many Line Items
        public ICollection<InvoiceLineItem>? InvoiceLineItems { get; set; } 
        // Navigation property: An Invoice has Payment Terms
        public PaymentTerms? PaymentTerms { get; set; } = null;


    }
}
