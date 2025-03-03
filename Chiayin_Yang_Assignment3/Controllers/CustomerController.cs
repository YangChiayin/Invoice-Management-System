using Chiayin_Yang_Assignment3.Data;
using Invoicing.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Chiayin_Yang_Assignment3.Controllers
{
    public class CustomerController : Controller
    {
        private InvoicesDbContext _invoicesDbContext;
        public CustomerController(InvoicesDbContext invoicesDbContext)
        {
            _invoicesDbContext = invoicesDbContext;
        }

        //fetches all non-deleted customers from the database asynchronously and returns them to the Index view.
        public async Task<IActionResult> Index()
        {
            return View(await _invoicesDbContext.Customers.Where(c => !c.IsDeleted).ToListAsync());
        }

        //A GET handler that returns the blank form to add a new customer:
        [HttpGet()]
        public IActionResult Add()
        {
            return View(new Customer());
        }
        // A POST handler that gets the customer data in the HTTP POST body passed
        // as a param and is then added to the DB:
        [HttpPost()]
        public IActionResult Add(Customer customer)
        {
            if (ModelState.IsValid)
            {
                // because it's valid add it to the DB & save changes:
                _invoicesDbContext.Customers.Add(customer);
                _invoicesDbContext.SaveChanges();

                // redirect back to the all customer view:
                return RedirectToAction("Index", "Customer");
            }
            else
            {
                return View(customer);
            }
        }

        [HttpGet()]
        public IActionResult Edit(int id)
        {
            var customer = _invoicesDbContext.Customers.Find(id);

            return View(customer);
        }
        [HttpPost()]
        public IActionResult Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                _invoicesDbContext.Customers.Update(customer);
                _invoicesDbContext.SaveChanges();

                return RedirectToAction("Index", "Customer");
            }
            else
            {
                return View(customer);
            }
        }

        //Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _invoicesDbContext.Customers
                .FirstOrDefaultAsync(c => c.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var customer = await _invoicesDbContext.Customers.FindAsync(id);

            if (customer != null)
            {
                customer.IsDeleted = true;

                await _invoicesDbContext.SaveChangesAsync();

                // Save the customer name in TempData for the undo message
                TempData["DeleteMessage"] = $"The customer \"{customer.Name}\" was deleted.";
            }

            TempData["DeletedItem"] = id;
            return RedirectToAction(nameof(Index));
        }

        // Undo the softdelete
        [HttpPost]
        public async Task<IActionResult> UndoDelete(int id)
        {
            var customer = await _invoicesDbContext.Customers.FindAsync(id);
            if (customer != null)
            {
                customer.IsDeleted = false;
                await _invoicesDbContext.SaveChangesAsync();
            }


            return RedirectToAction(nameof(Index));
        }

        // Permanently delete items after some delay time
        public async Task<IActionResult> DeleteConfirmed()
        {
            var itemsToDelete = _invoicesDbContext.Customers.Where(c => c.IsDeleted).ToList();

            if (itemsToDelete.Any())
            {
                _invoicesDbContext.Customers.RemoveRange(itemsToDelete);
                await _invoicesDbContext.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
        private bool CustomerExists(int id)
        {
            return _invoicesDbContext.Customers.Any(c => c.CustomerId == id);
        }
    }
}
