using Xunit;
using Moq;
using System.ComponentModel.DataAnnotations;  // Add this line
using Microsoft.EntityFrameworkCore;
using Chiayin_Yang_Assignment3.Data;
using Chiayin_Yang_Assignment3.Services;
using Invoicing.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Invoicing.Tests
{
    public class InvoiceServiceTests
    {
        private DbContextOptions<InvoicesDbContext> CreateNewContextOptions()
        {
            return new DbContextOptionsBuilder<InvoicesDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task GetCustomerWithInvoicesAsync_ReturnsCustomer()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new InvoicesDbContext(options))
            {
                var customer = new Customer
                {
                    CustomerId = 1,
                    Name = "Test Customer",
                    Address1 = "123 Test St",
                    City = "Test City",
                    ProvinceOrState = "CA",
                    ZipOrPostalCode = "12345",
                    Phone = "(123) 456-7890"
                };
                context.Customers.Add(customer);
                await context.SaveChangesAsync();

                var service = new InvoiceService(context);

                // Act
                var result = await service.GetCustomerWithInvoicesAsync(1);

                // Assert
                Assert.NotNull(result);
                Assert.Equal("Test Customer", result.Name);
            }
        }

        [Fact]
        public async Task GetInvoicesForCustomerAsync_ReturnsInvoices()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new InvoicesDbContext(options))
            {
                var customer = new Customer
                {
                    CustomerId = 1,
                    Name = "Test Customer",
                    Address1 = "123 Test St",
                    City = "Test City",
                    ProvinceOrState = "CA",
                    ZipOrPostalCode = "12345",
                    Phone = "(123) 456-7890"
                };
                context.Customers.Add(customer);

                var invoice = new Invoice
                {
                    InvoiceId = 1,
                    CustomerId = 1,
                    InvoiceDate = DateTime.Now,
                    PaymentTermsId = 1
                };
                context.Invoices.Add(invoice);
                await context.SaveChangesAsync();

                var service = new InvoiceService(context);

                // Act
                var result = await service.GetInvoicesForCustomerAsync(1);

                // Assert
                Assert.Single(result);
                Assert.Equal(1, result[0].CustomerId);
            }
        }

        [Fact]
        public void Customer_ValidationRules_WorkCorrectly()
        {
            // Arrange
            var customer = new Customer
            {
                Name = "",
                ProvinceOrState = "ABC", // Invalid state code
                Phone = "invalid",
                ZipOrPostalCode = "invalid"
            };

            // Act
            var context = new ValidationContext(customer);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(customer, context, results, true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(results, r => r.MemberNames.Contains("Name"));
            Assert.Contains(results, r => r.MemberNames.Contains("ProvinceOrState"));
            Assert.Contains(results, r => r.MemberNames.Contains("Phone"));
            Assert.Contains(results, r => r.MemberNames.Contains("ZipOrPostalCode"));
        }
    }
}