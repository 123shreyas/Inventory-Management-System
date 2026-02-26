using System;
using InventoryManagementSystem.Data;
using InventoryManagementSystem.Services;
using InventoryManagementSystem.ConsoleUI;
using Microsoft.EntityFrameworkCore;

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

        while (true)
        {
            ShowHeader();

            Console.WriteLine("MAIN MENU");
            Console.WriteLine("1. Products");
            Console.WriteLine("2. Categories");
            Console.WriteLine("3. Suppliers");
            Console.WriteLine("4. Warehouses");
            Console.WriteLine("5. Stock Management");
            Console.WriteLine("6. Stock Transactions");
            Console.WriteLine("7. Reports & Analytics");
            Console.WriteLine("0. Exit");
            Console.Write("\nSelect an option: ");

            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    Console.Clear();
                    new ProductConsole(productService).ShowMenu();
                    break;

                case "2":
                    Console.Clear();
                    new CategoryConsole(categoryService).ShowMenu();
                    break;

                case "3":
                    Console.Clear();
                    new SupplierConsole(supplierService).ShowMenu();
                    break;

                case "4":
                    Console.Clear();
                    new WarehouseConsole(warehouseService).ShowMenu();
                    break;

                case "5":
                    Console.Clear();
                    new StockConsole(stockService, context).ShowMenu();
                    break;

                case "6":
                    Console.Clear();
                    new StockTransactionConsole(context).ShowMenu();
                    break;

                case "7":
                    Console.Clear();
                    new ReportsConsole(stockService, context).ShowMenu();
                    break;

                case "0":
                    Console.Clear();
                    Console.WriteLine("Exiting Inventory Management System...");
                    return;

                default:
                    Console.WriteLine("\nInvalid option.");
                    Pause();
                    break;
            }
        }
    }

    // =============================================
    // Helper Methods
    // =============================================

    static void ShowHeader()
    {
        Console.Clear();
        Console.WriteLine("======================================");
        Console.WriteLine("      INVENTORY MANAGEMENT SYSTEM     ");
        Console.WriteLine("======================================\n");
    }

    public static void Pause()
    {
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }
}