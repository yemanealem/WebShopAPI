# WebShopAPI

A simple **.NET 10 Web API** for managing an online webshop. The API provides full CRUD (Create, Read, Update, Delete) operations for **Customers**, **Products**, and **Shopping Baskets**. It is designed to integrate with a React frontend and uses **SQLite** as the database provider. **Swagger** is included for API documentation and testing.

> **Note:** This project was developed as a lab assignment for a Full-Stack Development course.

---

## Technologies Used

- .NET 10 / ASP.NET Core Web API
- C# 12
- Entity Framework Core (EF Core)
- SQLite
- Swashbuckle (Swagger/OpenAPI)
- Visual Studio Code (VS Code)

---

## Project Structure

```text
WebShopAPI/
│
├── Controllers/       # API controllers (Customers, Products, Baskets)
├── Data/              # Database context (DbContext)
├── Models/            # Entity models
├── Migrations/        # EF Core migrations
├── Program.cs         # Application entry point
├── WebShopAPI.csproj
└── README.md
```

---

## Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/yemanealem/WebShopAPI.git
cd WebShopAPI
```

### 2. Install EF Core Tools

```bash
dotnet tool install --global dotnet-ef
```

### 3. Restore Dependencies

```bash
dotnet restore
```

### 4. Install Required Packages

```bash
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Swashbuckle.AspNetCore
```

### 5. Create and Apply Database Migrations

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 6. Run the Application

```bash
dotnet run
```

The API will be available at:

```text
http://localhost:5062
```

Swagger UI:

```text
http://localhost:5062/swagger
```

---

## API Endpoints

### Customers

| Method | Endpoint              | Description           |
| ------ | --------------------- | --------------------- |
| GET    | `/api/customers`      | Get all customers     |
| GET    | `/api/customers/{id}` | Get a customer by ID  |
| POST   | `/api/customers`      | Create a new customer |
| PUT    | `/api/customers/{id}` | Update a customer     |
| DELETE | `/api/customers/{id}` | Delete a customer     |

### Products

| Method | Endpoint             | Description          |
| ------ | -------------------- | -------------------- |
| GET    | `/api/products`      | Get all products     |
| GET    | `/api/products/{id}` | Get a product by ID  |
| POST   | `/api/products`      | Create a new product |
| PUT    | `/api/products/{id}` | Update a product     |
| DELETE | `/api/products/{id}` | Delete a product     |

### Shopping Baskets

| Method | Endpoint            | Description              |
| ------ | ------------------- | ------------------------ |
| GET    | `/api/baskets`      | Get all basket entries   |
| GET    | `/api/baskets/{id}` | Get a basket entry by ID |
| POST   | `/api/baskets`      | Add a new basket entry   |
| PUT    | `/api/baskets/{id}` | Update a basket entry    |
| DELETE | `/api/baskets/{id}` | Delete a basket entry    |

---

## Features

- RESTful API design
- CRUD operations for Customers, Products, and Shopping Baskets
- SQLite database integration using Entity Framework Core
- Automatic API documentation with Swagger
- Ready for integration with a React frontend
- Code-first database migrations

---

## Author

**Yemane Alem**
