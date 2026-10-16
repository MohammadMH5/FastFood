using FastFood.Api.Data;
using FastFood.Api.Middleware;
using FastFood.Api.Models;
using FastFood.Api.Repositories;
using FastFood.Api.Services.Implementations;
using FastFood.Api.Services.Interfaces;
using FastFood.Api.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace FastFood.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition(
                    "Bearer",
                    new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description = "Enter your JWT token."
                    });

                options.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                    });
            });

            // Database
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString(
                        "DefaultConnection")));

            // Repository
            builder.Services.AddScoped(
                typeof(IGenericRepository<>),
                typeof(GenericRepository<>));

            // UnitOfWork
            builder.Services.AddScoped<
                IUnitOfWork,
                FastFood.Api.UnitOfWork.UnitOfWork>();

            // Services
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();

            // Password Hasher
            builder.Services.AddScoped<
                PasswordHasher<ApplicationUser>>();

            // Authentication - JWT
            builder.Services.AddAuthentication(
                JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,

                            ValidIssuer =
                                builder.Configuration["Jwt:Issuer"],

                            ValidAudience =
                                builder.Configuration["Jwt:Audience"],

                            IssuerSigningKey =
                                new SymmetricSecurityKey(
                                    Encoding.UTF8.GetBytes(
                                        builder.Configuration["Jwt:Key"]!))
                        };
                });

            // Authorization
            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Global Exception Handling
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            // Static Files
            app.UseStaticFiles();

            // Seed Admin
            // Seed test users only in Development
            if (app.Environment.IsDevelopment())
            {
                using var scope = app.Services.CreateScope();

                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDbContext>();
                var passwordHasher = services.GetRequiredService<PasswordHasher<ApplicationUser>>();
                var configuration = services.GetRequiredService<IConfiguration>();

                await AdminSeeder.SeedAsync(
                    context,
                    passwordHasher,
                    configuration);
            }

            // Configure the HTTP request pipeline.

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Authentication باید قبل از Authorization باشد
            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}