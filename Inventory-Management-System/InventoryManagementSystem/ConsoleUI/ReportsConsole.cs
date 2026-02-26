using InventoryManagementSystem.Data;
using InventoryManagementSystem.Services;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.ConsoleUI
{
    public class ReportsConsole
    {
        private readonly StockService _stockService;
        private readonly InventoryContext _context;

        public ReportsConsole(StockService stockService, InventoryContext context)
        {
            _stockService = stockService;
            _context = context;
        }

        public void ShowMenu()
        {
            while (true)
            {
                
                Console.WriteLine("\n--- REPORTS & ANALYTICS ---");
                Console.WriteLine("1. Total Inventory Value");
                Console.WriteLine("2. Product-wise Stock Summary");
                Console.WriteLine("3. Warehouse-wise Stock Summary");
                Console.WriteLine("0. Back");
                Console.Write("Select: ");

                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        ShowTotalInventoryValue();
                        break;

                    case "2":
                        ShowProductWiseStock();
                        break;

                    case "3":
                        ShowWarehouseWiseStock();
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        private void ShowTotalInventoryValue()
        {
            var total = _stockService.GetTotalInventoryValue();
            Console.WriteLine($"\nTotal Inventory Value: {total:C}");
        }

        private void ShowProductWiseStock()
        {
            var products = _context.Products
                .Include(p => p.StockLevels)
                .ToList();

            if (!products.Any())
            {
                Console.WriteLine("No products found.");
                return;
            }

            Console.WriteLine("\n--- PRODUCT STOCK SUMMARY ---");

            foreach (var p in products)
            {
                int qty = p.StockLevels.Sum(sl => sl.QuantityOnHand);
                Console.WriteLine($"{p.ProductName} → Stock: {qty}");
            }
        }

        private void ShowWarehouseWiseStock()
        {
            var warehouses = _context.Warehouses
                .Include(w => w.StockLevels)
                .ThenInclude(sl => sl.Product)
                .ToList();

            if (!warehouses.Any())
            {
                Console.WriteLine("No warehouses found.");
                return;
            }

            Console.WriteLine("\n--- WAREHOUSE STOCK SUMMARY ---");

            foreach (var w in warehouses)
            {
                Console.WriteLine($"\nWarehouse: {w.WarehouseName}");

                if (!w.StockLevels.Any())
                {
                    Console.WriteLine("  No stock available.");
                    continue;
                }

                foreach (var sl in w.StockLevels)
                {
                    Console.WriteLine($"  - {sl.Product?.ProductName}: {sl.QuantityOnHand}");
                }
            }
        }
    }
}