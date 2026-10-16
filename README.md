FastFood – Online Food Ordering System
A web-based food ordering application built with C# and ASP.NET Core.
Overview
FastFood is a portfolio project designed to demonstrate backend development, RESTful API design, authentication, database integration, and web application development.
The solution consists of two projects:

FastFood.Api: Backend REST API.
FastFood.Web: Web interface built with ASP.NET Core Razor Pages.

Technologies

C#
.NET 8
ASP.NET Core Web API
ASP.NET Core Razor Pages
Entity Framework Core
SQL Server
JWT Authentication
Cookie Authentication
HTML, CSS, and Bootstrap

Backend Features

RESTful API endpoints
Layered architecture
Repository and Unit of Work patterns
Entity Framework Core and SQL Server integration
Product and category management
JWT-based authentication and role-based authorization
Shopping cart management, including adding, updating, and removing items
Cart total calculation
Order creation and management
Payment-related API functionality (not connected to a real payment gateway)

Web Features

Persian, right-to-left (RTL) user interface
Dynamic product and category listings
Product images, descriptions, prices, and availability
Login and logout functionality
JWT storage in server-side session
Cookie-based authentication
Shopping cart operations and order workflow integrated with the backend API

Project Structure
FastFood/
├── FastFood.Api/
│   ├── Controllers/
│   ├── DTOs/
│   ├── Models/
│   ├── Repositories/
│   ├── Services/
│   └── wwwroot/
│
└── FastFood.Web/
    ├── Pages/
    ├── Models/
    ├── Services/
    └── wwwroot/

Getting Started
Prerequisites

.NET 8 SDK
SQL Server
Visual Studio 2022 or another compatible IDE

Setup

Clone the repository.
Configure the database connection string using User Secrets or environment variables.
Configure the JWT signing key and other required settings securely.
Apply the required Entity Framework Core migrations.
Run FastFood.Api.
Run FastFood.Web.

Ensure that the API URL configured in the Web project matches the address where the API is running.
Project Status
FastFood implements the core functionality of a food ordering application, including product and category management, authentication, shopping cart operations, order creation, and payment-related API functionality.
The Web application provides a Persian RTL interface, dynamic product and category listings, login/logout functionality, and shopping cart and order workflows.
Note: The project is not connected to a real payment gateway. Payment-related functionality is limited to the implemented API features.
Security

Do not commit passwords, JWT signing keys, connection strings containing credentials, or real user data.
Use environment-specific configuration and secure secret storage.
Review configuration files before publishing the repository.

Author
Developed as a portfolio project to practice backend development with C# and ASP.NET Core.
