using System.ComponentModel.DataAnnotations;

namespace Invoicing.Entities
{
    public class Customer
    {
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Please provide a Name")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Please provide Address 1")]
        public string? Address1 { get; set; }

        public string? Address2 { get; set; }

        [Required(ErrorMessage = "Please provide City")]
        public string? City { get; set; } = null!;

        [Required(ErrorMessage = "Please provide Province/State")]
        [RegularExpression(@"^[A-Za-z]{2}$", ErrorMessage = "Province/State must be a 2-letter code.")]
        public string? ProvinceOrState { get; set; } = null!;

        [Required(ErrorMessage = "Please provide Zip/Postal code")]
        [RegularExpression(@"^\d{5}(-\d{4})?$|^[A-Za-z]\d[A-Za-z] ?\d[A-Za-z]\d$", ErrorMessage = "Invalid Zip or Postal Code format.")]
        public string? ZipOrPostalCode { get; set; } = null!;

        [Required(ErrorMessage = "Please provide Phone")]
        [RegularExpression(@"^\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4}$", ErrorMessage = "Invalid phone number.")]
        public string? Phone { get; set; }

        public string? ContactLastName { get; set; }

        public string? ContactFirstName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string? ContactEmail { get; set; }

        public bool IsDeleted { get; set; } = false;

        // Navigation property: A Customer can have many Invoices //
        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    }
}
