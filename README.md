# Inventory Management System

## Table of Contents

1. [Project Overview](#project-overview)
2. [Features](#features)
3. [Technologies Used](#technologies-used)
4. [Database Design](#database-design)

   * [Entity Relationship Diagram (ERD)](#entity-relationship-diagram-erd)
   * [Relationships](#relationships)
5. [Installation](#installation)
6. [Running the Application](#running-the-application)
7. [Database Migrations](#database-migrations)
8. [Usage](#usage)
9. [Contributing](#contributing)
10. [License](#license)

---

## Project Overview

The **Inventory Management System (IMS)** is a console-based application developed in **C#** using **.NET Core** and **Entity Framework Core**.
It allows businesses to manage products, categories, suppliers, and stock levels efficiently. The application is modular, using a **service-oriented architecture**, with separate layers for models, services, and console UI.

Key modules include:

* **Product Management** – Add, update, deactivate, and view products.
* **Category Management** – Add, view, and analyze categories.
* **Supplier Management** – Add, view, and manage suppliers.
* **ABC Analysis** – Categorize products based on consumption value for inventory optimization.

---

## Features

* Add, view, update, and deactivate products
* Manage product categories and suppliers
* ABC Analysis of inventory
* Console-based navigation menu
* Entity Framework Core integration for database operations
* Migrations for schema management

---

## Technologies Used

* **C#** (.NET 7 / .NET Core)
* **Entity Framework Core** (Code-First approach)
* **SQL Server** (or any relational DB supported by EF Core)
* **Console Application**

---

## Database Design

The system uses a **relational database** with the following tables:

### Tables

1. **Product**

   * `ProductId` (PK) – int
   * `SKU` – string
   * `ProductName` – string
   * `Description` – string
   * `CategoryId` (FK) – int
   * `UnitOfMeasure` – string
   * `Cost` – decimal
   * `Price` – decimal
   * `ReorderLevel` – int
   * `IsActive` – bool

2. **ProductCategory**

   * `CategoryId` (PK) – int
   * `CategoryName` – string
   * `Description` – string

3. **Supplier**

   * `SupplierId` (PK) – int
   * `Name` – string
   * `Email` – string
   * `Phone` – string

4. **SupplierProduct** (many-to-many)

   * `SupplierId` (FK) – int
   * `ProductId` (FK) – int
   * `Cost` – decimal
   * `Price` – decimal

### Entity Relationship Diagram (ERD)

```
ProductCategory 1 --- * Product * --- * Supplier
```

* One **Category** can have many **Products**.
* One **Product** can be supplied by many **Suppliers** (Many-to-Many).

---

## Relationships

* **Product ↔ ProductCategory** – One-to-Many
* **Product ↔ Supplier** – Many-to-Many via `SupplierProduct` table

EF Core handles relationships using **navigation properties** in the models:

```csharp
public class Product
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string? SKU { get; set; }
    public string? Description { get; set; }
    public int CategoryId { get; set; }
    public ProductCategory? Category { get; set; }
    public string? UnitOfMeasure { get; set; }
    public decimal Cost { get; set; }
    public decimal Price { get; set; }
    public int ReorderLevel { get; set; }
    public bool IsActive { get; set; } = true;
}
```

---

## Installation

1. Clone the repository:

```bash
git clone https://github.com/yourusername/InventoryManagementSystem.git
cd InventoryManagementSystem
```

2. Ensure **.NET SDK 7+** is installed. Check:

```bash
dotnet --version
```

3. Restore NuGet packages:

```bash
dotnet restore
```

---

## Running the Application

Run the console application using:

```bash
dotnet run --project InventoryManagementSystem.ConsoleUI
```

---

## Database Migrations

The project uses **Entity Framework Core Code-First** approach.

### Common Commands

* **Add Migration**

```bash
dotnet ef migrations add InitialCreate
```

* **Update Database**

```bash
dotnet ef database update
```

* **List Migrations**

```bash
dotnet ef migrations list
```

* **Remove Last Migration** (if not applied)

```bash
dotnet ef migrations remove
```

> Make sure to install EF Core tools globally if not already:

```bash
dotnet tool install --global dotnet-ef
```

---

## Usage

Once the application is running, you will see a console menu for:

1. Product Management
2. Category Management
3. Supplier Management
4. ABC Analysis

Select an option by entering the corresponding number and follow prompts.

Example:

```
--- PRODUCTS ---
1. Add Product
2. View All Products
3. View Product By ID
4. Update Product
5. Deactivate Product
6. Back to Main Menu
Select an option:
```

---

## Contributing

1. Fork the repository
2. Create a new branch (`git checkout -b feature-name`)
3. Make changes
4. Commit (`git commit -m "Add feature"`)
5. Push (`git push origin feature-name`)
6. Create a Pull Request

---

## License

This project is licensed under the **MIT License** – see the LICENSE file for details.
