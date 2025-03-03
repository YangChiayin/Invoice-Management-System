using Chiayin_Yang_Assignment3.Data;
using Chiayin_Yang_Assignment3.Services;
using Microsoft.EntityFrameworkCore;

namespace Chiayin_Yang_Assignment3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Register the InvoiceService as a singleton or scoped service..
            builder.Services.AddScoped<IInvoiceService, InvoiceService>();

            // add our context as a service:
            string connStr = builder.Configuration.GetConnectionString("InvoicesDb");
            builder.Services.AddDbContext<InvoicesDbContext>(options => options.UseSqlServer(connStr));


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
