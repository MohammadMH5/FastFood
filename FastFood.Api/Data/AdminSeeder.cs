using FastFood.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Api.Data
{
    public static class AdminSeeder
    {
        public static async Task SeedAsync(
            ApplicationDbContext context,
            PasswordHasher<ApplicationUser> passwordHasher,
            IConfiguration configuration)
        {
            var adminUsername = configuration["SeedUsers:Admin:Username"];
            var adminPassword = configuration["SeedUsers:Admin:Password"];
            var customerUsername = configuration["SeedUsers:Customer:Username"];
            var customerPassword = configuration["SeedUsers:Customer:Password"];

            if (string.IsNullOrWhiteSpace(adminUsername) ||
                string.IsNullOrWhiteSpace(adminPassword) ||
                string.IsNullOrWhiteSpace(customerUsername) ||
                string.IsNullOrWhiteSpace(customerPassword))
            {
                throw new InvalidOperationException(
                    "تنظیمات حساب‌های آزمایشی در User Secrets کامل نیست.");
            }

            if (await context.Users.AnyAsync(u => u.Username == adminUsername) == false)
            {
                var admin = new ApplicationUser
                {
                    Username = adminUsername,
                    Role = "Admin"
                };

                admin.PasswordHash = passwordHasher.HashPassword(
                    admin,
                    adminPassword);

                await context.Users.AddAsync(admin);
            }

            if (await context.Users.AnyAsync(u => u.Username == customerUsername) == false)
            {
                var customer = new ApplicationUser
                {
                    Username = customerUsername,
                    Role = "Customer"
                };

                customer.PasswordHash = passwordHasher.HashPassword(
                    customer,
                    customerPassword);

                await context.Users.AddAsync(customer);
            }

            await context.SaveChangesAsync();
        }
    }
}