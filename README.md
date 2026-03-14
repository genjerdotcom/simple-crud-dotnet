# Product API (.NET Clean Architecture)

Simple CRUD REST API built with **.NET**, **Entity Framework Core**, **SQLite**, **FluentValidation**, and **Clean Architecture** principles.

The project demonstrates:

* Modular **feature-based architecture**
* **DTO + Validator**
* **UseCase / Application layer**
* **Repository pattern with BaseRepository**
* **Middleware request logging**
* **Swagger documentation**
* **Environment-based logging configuration**

---

# Requirements

Before running the project make sure you have:

* .NET SDK **10.0.201**
* Git
* SQLite (optional, EF will create DB automatically)

Check .NET version:

```bash
dotnet --version
```

Expected:

```
10.0.201
```

---

# Project Structure

```
.
├── Core
│   ├── Entities
│   ├── Interfaces
│   └── Responses
│
├── Features
│   └── Products
│       ├── Controllers
│       ├── DTO
│       ├── Repository
│       ├── UseCases
│       └── Validators
│
├── Infrastructure
│   ├── Database
│   └── Repositories
│
├── Middlewares
│
├── Program.cs
└── appsettings.json
```

Architecture layers:

| Layer          | Purpose                      |
| -------------- | ---------------------------- |
| Core           | Domain entities & interfaces |
| Features       | Business logic per feature   |
| Infrastructure | Database & repositories      |
| Middlewares    | Global HTTP middleware       |
| Controllers    | HTTP entry points            |

---

# Installation

Clone the repository:

```bash
git clone https://github.com/your-repo/simple-crud-dotnet.git
cd simple-crud-dotnet
```

Restore dependencies:

```bash
dotnet restore
```

---

# Database Setup

This project uses **SQLite with Entity Framework Core**.

Connection string:

```
Data Source=app.db
```

### Create Migration

```bash
dotnet ef migrations add InitialCreate
```

### Apply Migration

```bash
dotnet ef database update
```

This will create the database:

```
app.db
```

---

# Running the Application

Run the API:

```bash
dotnet run
```

Default server:

```
https://localhost:5093
```

---

# Swagger API Documentation

Open in browser:

```
http://localhost:5093/swagger
```

You can test the API directly from Swagger UI.

---

# API Endpoints

## Create Product

POST `/api/products`

Body:

```json
{
  "name": "Laptop",
  "price": 1200
}
```

---

## Get All Products

GET `/api/products`

---

## Get Product By Id

GET `/api/products/{id}`

Example:

```
GET /api/products/1
```

---

## Update Product

PUT `/api/products/{id}`

Body:

```json
{
  "name": "Updated Laptop",
  "price": 1500
}
```

---

## Delete Product

DELETE `/api/products/{id}`

---

# Validation

Validation is implemented using **FluentValidation**.

Example rules:

* Product name minimum length **3**
* Product price must be **greater than 0**

Invalid requests automatically return:

```
400 Bad Request
```

---

# Logging

Logging uses **Microsoft.Extensions.Logging**.

Each request includes:

* Log level
* Timestamp

Log levels can be configured per environment:

```
appsettings.Development.json
appsettings.Staging.json
appsettings.Production.json
```

---

# Tech Stack

* .NET 10
* ASP.NET Core
* Entity Framework Core
* SQLite
* FluentValidation
* Swagger
* Clean Architecture

---

# Environment Configuration

ASP.NET automatically loads environment config:

```
ASPNETCORE_ENVIRONMENT
```

Example:

```
Development
Staging
Production
```

Run with environment:

```bash
ASPNETCORE_ENVIRONMENT=Development dotnet run
```

---

# Useful Commands

Restore packages:

```bash
dotnet restore
```

Run project:

```bash
dotnet run

Or

ASPNETCORE_ENVIRONMENT=Development dotnet run

Or

ASPNETCORE_ENVIRONMENT=Staging dotnet run

Or

ASPNETCORE_ENVIRONMENT=Production dotnet run
```

Create migration:

```bash
dotnet ef migrations add MigrationName
```

Update database:

```bash
dotnet ef database update
```

Remove last migration:

```bash
dotnet ef migrations remove
```

---


# Author

Ginanjar Dwi Putranto
