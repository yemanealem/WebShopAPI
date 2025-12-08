WebShopAPI

A simple .NET 10 Web API for managing a web shop. Provides CRUD operations for Customers, Products, and Shopping Baskets. Designed to integrate with a React frontend. Uses SQLite for storage and Swagger for API documentation/testing.

Note: This project is a lab assignment for a Full-Stack Development course.


Technologies Used

.NET 10 / ASP.NET Core Web API

Entity Framework Core (EF Core)

SQLite Database

Swashbuckle (Swagger)

Visual Studio Code (VS Code)

C# 12

WebShopAPI/
│
├─ Controllers/           # API Controllers (Customer, Product, Basket)
├─ Data/                  # DbContext class
├─ Models/                # Entity models (Customer, Product, ShoppingBasket)
├─ Migrations/            # EF Core migrations
├─ Program.cs             # Application entry point
├─ WebShopAPI.csproj
└─ README.md

Setup Instructions

1) git clone git remote add origin https://github.com/yemanealem/WebShopAPI.git
cd WebShopAPI

2) dotnet tool install --global dotnet-ef

3)  dotnet restore
Required packages:
4) dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Swashbuckle.AspNetCore

5. Create and apply migrations
dotnet ef migrations add InitialCreate
dotnet ef database update

6) dotnet run

http://localhost:5062

and Access API documentation and test endpoints:

http://localhost:5062/swagger


API Endpoints
Customers

GET /api/customers → Get all customers

GET /api/customers/{id} → Get customer by ID

POST /api/customers → Create a new customer

PUT /api/customers/{id} → Update customer

DELETE /api/customers/{id} → Delete customer

Products

GET /api/products → Get all products

GET /api/products/{id} → Get product by ID

POST /api/products → Add new product

PUT /api/products/{id} → Update product

DELETE /api/products/{id} → Delete product

Shopping Baskets

GET /api/baskets → Get all basket entries

GET /api/baskets/{id} → Get basket by ID

POST /api/baskets → Add basket entry

PUT /api/baskets/{id} → Update basket entry

DELETE /api/baskets/{id} → Delete basket entry



