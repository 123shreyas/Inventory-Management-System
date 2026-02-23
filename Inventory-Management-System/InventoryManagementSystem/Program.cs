using System;
using System.Linq;
using InventoryManagementSystem.Data;
using InventoryManagementSystem.Models;
using InventoryManagementSystem.Services;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

class Program
{
    static void Main(string[] args)
    {
        var options = new DbContextOptionsBuilder<InventoryContext>()
            .UseMySql(
                "server=localhost;database=InventoryDB;user=root;password=Shreyas@123",
                new MySqlServerVersion(new Version(8, 0, 36))
            )
            .Options;

        using var context = new InventoryContext(options);

        var productService = new ProductService(context);
        var categoryService = new CategoryService(context);
        var stockService = new StockService(context);
        var supplierService = new SupplierService(context);
        var warehouseService = new WarehouseService(context);

        Console.WriteLine("======================================");
        Console.WriteLine("      INVENTORY MANAGEMENT SYSTEM      ");
        Console.WriteLine("======================================");

        while (true)
        {
            Console.WriteLine("\nMAIN MENU");
            Console.WriteLine("1. Products");
            Console.WriteLine("2. Categories");
            Console.WriteLine("3. Suppliers");
            Console.WriteLine("4. Warehouses");
            Console.WriteLine("5. Stock Management");
            Console.WriteLine("6. Stock Transactions");
            Console.WriteLine("7. Reports & Analytics");
            Console.WriteLine("8. Exit");
            Console.Write("Select an option: ");

            var input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    ProductMenu(productService);
                    break;
                case "2":
                    CategoryMenu(categoryService);
                    break;
                case "3":
                    SupplierMenu(supplierService, productService);
                    break;
                case "4":
                    WarehouseMenu(warehouseService);
                    break;
                case "5":
                    StockMenu(stockService, context);
                    break;
                case "6":
                    TransactionMenu(context);
                    break;
                case "7":
                    ReportsMenu(stockService, context);
                    break;
                case "8":
                    return;
                default:
                    Console.WriteLine("Invalid option, try again.");
                    break;
            }
        }
    }

    #region Product Menu
    static void ProductMenu(ProductService productService)
    {
        while (true)
        {
            Console.WriteLine("\n--- PRODUCTS ---");
            Console.WriteLine("1. Add Product");
            Console.WriteLine("2. View All Products");
            Console.WriteLine("3. View Product By ID");
            Console.WriteLine("4. Update Product");
            Console.WriteLine("5. Deactivate Product");
            Console.WriteLine("6. Back to Main Menu");
            Console.Write("Select an option: ");
            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    Console.Write("Enter Name: ");
                    string name = Console.ReadLine()!;
                    Console.Write("Enter SKU: ");
                    string sku = Console.ReadLine()!;
                    Console.Write("Enter Category ID: ");
                    int catId = int.Parse(Console.ReadLine()!);
                    Console.Write("Enter Cost: ");
                    decimal cost = decimal.Parse(Console.ReadLine()!);
                    Console.Write("Enter List Price: ");
                    decimal price = decimal.Parse(Console.ReadLine()!);
                    Console.Write("Enter Reorder Level: ");
                    int reorder = int.Parse(Console.ReadLine()!);

                    productService.AddProduct(new Product
                    {
                        ProductName = name,
                        SKU = sku,
                        CategoryId = catId,
                        Cost = cost,
                        ListPrice = price,
                        ReorderLevel = reorder,
                        IsActive = true
                    });
                    break;

                case "2":
                    var products = productService.GetAllProducts();
                    Console.WriteLine("{0,-5} {1,-20} {2,-10} {3,-6} {4,-6}", "ID", "Name", "SKU", "Price", "Active");
                    foreach (var p in products)
                        Console.WriteLine("{0,-5} {1,-20} {2,-10} {3,-6} {4,-6}", p.ProductId, p.ProductName, p.SKU, p.ListPrice, p.IsActive);
                    break;

                case "3":
                    Console.Write("Enter Product ID: ");
                    int pid = int.Parse(Console.ReadLine()!);
                    var product = productService.GetProduct(pid);
                    if (product != null)
                        Console.WriteLine($"ID: {product.ProductId}, Name: {product.ProductName}, SKU: {product.SKU}, Price: {product.ListPrice}");
                    else
                        Console.WriteLine("Product not found.");
                    break;

                case "4":
                    Console.Write("Enter Product ID to update: ");
                    int upId = int.Parse(Console.ReadLine()!);
                    var toUpdate = productService.GetProduct(upId);
                    if (toUpdate != null)
                    {
                        Console.Write("Enter new Name: ");
                        toUpdate.ProductName = Console.ReadLine()!;
                        Console.Write("Enter new Price: ");
                        toUpdate.ListPrice = decimal.Parse(Console.ReadLine()!);
                        productService.UpdateProduct(toUpdate);
                    }
                    else
                        Console.WriteLine("Product not found.");
                    break;

                case "5":
                    Console.Write("Enter Product ID to deactivate: ");
                    int delId = int.Parse(Console.ReadLine()!);
                    productService.DeleteProduct(delId);
                    break;

                case "6":
                    return;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }
    #endregion

    #region Category Menu
    static void CategoryMenu(CategoryService categoryService)
    {
        while (true)
        {
            Console.WriteLine("\n--- CATEGORIES ---");
            Console.WriteLine("1. Add Category");
            Console.WriteLine("2. View All Categories");
            Console.WriteLine("3. ABC Analysis");
            Console.WriteLine("4. Back");
            Console.Write("Select: ");
            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    Console.Write("Enter Name: ");
                    string name = Console.ReadLine()!;
                    categoryService.AddCategory(new ProductCategory { CategoryName = name });
                    break;
                case "2":
                    var categories = categoryService.GetAllCategories();
                    foreach (var c in categories)
                        Console.WriteLine($"ID: {c.ProductCategoryId}, Name: {c.CategoryName}");
                    break;
                case "3":
                    categoryService.ABCAnalysis();
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }
    #endregion

  #region Supplier Menu
static void SupplierMenu(SupplierService supplierService, ProductService productService)
{
    while (true)
    {
        Console.WriteLine("\n--- SUPPLIERS ---");
        Console.WriteLine("1. Add Supplier");
        Console.WriteLine("2. View All Suppliers");
        Console.WriteLine("3. Back");
        Console.Write("Select: ");
        var input = Console.ReadLine();

        switch (input)
        {
            case "1":
                Console.Write("Enter Supplier Name: ");
                string name = Console.ReadLine()!;

                Console.Write("Enter Email (optional): ");
                string? email = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(email)) email = null;

                Console.Write("Enter Phone (optional): ");
                string? phone = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(phone)) phone = null;

                Console.Write("Enter Website (optional): ");
                string? website = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(website)) website = null;

                supplierService.AddSupplier(new Supplier
                {
                    SupplierName = name,
                    Email = email,
                    Phone = phone,
                    Website = website
                });
                break;

            case "2":
                var suppliers = supplierService.GetAllSuppliers();
                foreach (var s in suppliers)
                {
                    Console.WriteLine($"ID: {s.SupplierId}, Name: {s.SupplierName}, Email: {s.Email ?? "-"}, Phone: {s.Phone ?? "-"}, Website: {s.Website ?? "-"}");
                }
                break;

            case "3":
                return;

            default:
                Console.WriteLine("Invalid option.");
                break;
        }
    }
}
#endregion


    #region Warehouse Menu
    static void WarehouseMenu(WarehouseService warehouseService)
    {
        while (true)
        {
            Console.WriteLine("\n--- WAREHOUSES ---");
            Console.WriteLine("1. Add Warehouse");
            Console.WriteLine("2. View All Warehouses");
            Console.WriteLine("3. Back");
            Console.Write("Select: ");
            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    Console.Write("Enter Warehouse Name: ");
                    string name = Console.ReadLine()!;
                    warehouseService.AddWarehouse(new Warehouse { WarehouseName = name });
                    break;
                case "2":
                    var whs = warehouseService.GetAllWarehouses();
                    foreach (var w in whs)
                        Console.WriteLine($"ID: {w.WarehouseId}, Name: {w.WarehouseName}");
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }
    #endregion

    #region Stock Menu
    static void StockMenu(StockService stockService, InventoryContext context)
    {
        while (true)
        {
            Console.WriteLine("\n--- STOCK MANAGEMENT ---");
            Console.WriteLine("1. Stock In");
            Console.WriteLine("2. Stock Out");
            Console.WriteLine("3. Transfer Stock");
            Console.WriteLine("4. Adjust Stock");
            Console.WriteLine("5. Check Low Stock");
            Console.WriteLine("6. Back");
            Console.Write("Select: ");
            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    Console.Write("Product ID: "); int pid = int.Parse(Console.ReadLine()!);
                    Console.Write("Warehouse ID: "); int wid = int.Parse(Console.ReadLine()!);
                    Console.Write("Quantity: "); int qty = int.Parse(Console.ReadLine()!);
                    Console.Write("Reference: "); string refIn = Console.ReadLine()!;
                    stockService.StockIn(pid, wid, qty, refIn);
                    break;
                case "2":
                    Console.Write("Product ID: "); int pidOut = int.Parse(Console.ReadLine()!);
                    Console.Write("Warehouse ID: "); int widOut = int.Parse(Console.ReadLine()!);
                    Console.Write("Quantity: "); int qtyOut = int.Parse(Console.ReadLine()!);
                    Console.Write("Reference: "); string refOut = Console.ReadLine()!;
                    stockService.StockOut(pidOut, widOut, qtyOut, refOut);
                    break;
                case "3":
                    Console.Write("Product ID: "); int tpid = int.Parse(Console.ReadLine()!);
                    Console.Write("From Warehouse ID: "); int fwid = int.Parse(Console.ReadLine()!);
                    Console.Write("To Warehouse ID: "); int twid = int.Parse(Console.ReadLine()!);
                    Console.Write("Quantity: "); int tqty = int.Parse(Console.ReadLine()!);
                    stockService.TransferStock(tpid, fwid, twid, tqty);
                    break;
                case "4":
                    Console.Write("Product ID: "); int apid = int.Parse(Console.ReadLine()!);
                    Console.Write("Warehouse ID: "); int awid = int.Parse(Console.ReadLine()!);
                    Console.Write("New Quantity: "); int aqty = int.Parse(Console.ReadLine()!);
                    Console.Write("Reference: "); string aref = Console.ReadLine()!;
                    stockService.AdjustStock(apid, awid, aqty, aref);
                    break;
                case "5":
                    stockService.CheckLowStock();
                    break;
                case "6":
                    return;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }
    #endregion

    #region Transaction Menu
    static void TransactionMenu(InventoryContext context)
    {
        Console.WriteLine("\n--- STOCK TRANSACTIONS ---");
        var transactions = context.StockTransactions
            .Include(t => t.Product)
            .Include(t => t.Warehouse)
            .ToList();

        Console.WriteLine("{0,-5} {1,-15} {2,-15} {3,-10} {4,-8} {5,-20}", "ID", "Product", "Warehouse", "Qty", "Type", "Reference");
        foreach (var t in transactions)
            Console.WriteLine("{0,-5} {1,-15} {2,-15} {3,-10} {4,-8} {5,-20}",
                t.TransactionId, t.Product.ProductName, t.Warehouse.WarehouseName, t.Quantity, t.TransactionType, t.Reference);
    }
    #endregion

    #region Reports Menu
    static void ReportsMenu(StockService stockService, InventoryContext context)
    {
        while (true)
        {
            Console.WriteLine("\n--- REPORTS & ANALYTICS ---");
            Console.WriteLine("1. Total Inventory Value");
            Console.WriteLine("2. Product-wise Stock Summary");
            Console.WriteLine("3. Warehouse-wise Stock Summary");
            Console.WriteLine("4. Back");
            Console.Write("Select: ");
            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    Console.WriteLine($"Total Inventory Value: {stockService.GetTotalInventoryValue():C}");
                    break;
                case "2":
                    var products = context.Products
                        .Include(p => p.StockLevels)
                        .ToList();
                    foreach (var p in products)
                    {
                        int qty = p.StockLevels.Sum(sl => sl.QuantityOnHand);
                        Console.WriteLine($"{p.ProductName} → Stock: {qty}");
                    }
                    break;
                case "3":
                    var warehouses = context.Warehouses
                        .Include(w => w.StockLevels)
                        .ThenInclude(sl => sl.Product)
                        .ToList();
                    foreach (var w in warehouses)
                    {
                        Console.WriteLine($"Warehouse: {w.WarehouseName}");
                        foreach (var sl in w.StockLevels)
                            Console.WriteLine($" - {sl.Product.ProductName}: {sl.QuantityOnHand}");
                    }
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }
    #endregion
}
