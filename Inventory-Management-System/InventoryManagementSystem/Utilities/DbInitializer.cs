using InventoryManagementSystem.Data;
using InventoryManagementSystem.Models;

namespace InventoryManagementSystem.Utilities
{
    public static class DbInitializer
    {
        public static void Seed(InventoryContext context)
{
    context.Database.EnsureCreated();

    // ---------------- CATEGORY ----------------
    var category = context.ProductCategories
        .FirstOrDefault(c => c.CategoryName == "Medical");

    if (category == null)
    {
        category = new ProductCategory
        {
            CategoryName = "Medical",
            Description = "Medical Products"
        };

        context.ProductCategories.Add(category);
        context.SaveChanges();
        Console.WriteLine("Category Added.");
    }

    // ---------------- WAREHOUSES ----------------
    if (!context.Warehouses.Any(w => w.WarehouseName == "Main Store"))
    {
        context.Warehouses.Add(new Warehouse
        {
            WarehouseName = "Main Store",
            Location = "Solapur",
            Capacity = 1000
        });
        Console.WriteLine("Main Store Added.");
    }

    if (!context.Warehouses.Any(w => w.WarehouseName == "Backup Store"))
    {
        context.Warehouses.Add(new Warehouse
        {
            WarehouseName = "Backup Store",
            Location = "Pune",
            Capacity = 500
        });
        Console.WriteLine("Backup Store Added.");
    }

    context.SaveChanges();

    // ---------------- PRODUCT ----------------
    if (!context.Products.Any(p => p.SKU == "MED900"))
    {
        var product = new Product
        {
            SKU = "MED900",
            ProductName = "Paracetamol",
            CategoryId  = category.ProductCategoryId,
            Cost = 10,
            ListPrice = 15,
            IsActive = true,
            ReorderLevel = 60
        };

        context.Products.Add(product);
        context.SaveChanges();

        Console.WriteLine("Product Added.");
    }

    Console.WriteLine("Database Seeding Completed.");
}
    }
}
