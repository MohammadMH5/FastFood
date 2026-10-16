using System;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace FastFood.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            builder.Services.AddHttpClient<FastFood.Web.Services.ProductApiService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7153/");
            });
            builder.Services.AddHttpClient<FastFood.Web.Services.CategoryApiService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7153/");
            });
            
            builder.Services.AddHttpClient<FastFood.Web.Services.AuthApiService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7153/");
            });
            builder.Services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Login";
                    options.AccessDeniedPath = "/AccessDenied";

                    options.Cookie.Name = "FastFood.Auth";
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.Cookie.SameSite = SameSiteMode.Lax;

                    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                    options.SlidingExpiration = false;
                });

            builder.Services.AddAuthorization();

            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession(options =>
            {
                options.Cookie.Name = "FastFood.Session";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Lax;

                options.IdleTimeout = TimeSpan.FromMinutes(60);
            });
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<FastFood.Web.Services.CartService>();
            builder.Services.AddHttpClient<FastFood.Web.Services.CartApiService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7153/");
            });

            builder.Services.AddHttpClient<FastFood.Web.Services.OrderApiService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7153/");
            });



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();


        }
    }
}
