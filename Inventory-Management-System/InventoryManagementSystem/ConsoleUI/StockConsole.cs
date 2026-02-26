using InventoryManagementSystem.Models;
using InventoryManagementSystem.Services;
using InventoryManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.ConsoleUI
{
    public class StockConsole
    {
        private readonly StockService _stockService;
        private readonly InventoryContext _context;

        public StockConsole(StockService stockService, InventoryContext context)
        {
            _stockService = stockService;
            _context = context;
        }

        public void ShowMenu()
        {
            while (true)
            {
                
                Console.WriteLine("\n--- STOCK MANAGEMENT ---");
                Console.WriteLine("1. Stock In");
                Console.WriteLine("2. Stock Out");
                Console.WriteLine("3. Transfer Stock");
                Console.WriteLine("4. Adjust Stock");
                Console.WriteLine("5. Check Low Stock");
                Console.WriteLine("0. Back");
                Console.Write("Select: ");

                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        StockIn();
                        break;

                    case "2":
                        StockOut();
                        break;

                    case "3":
                        TransferStock();
                        break;

                    case "4":
                        AdjustStock();
                        break;

                    case "5":
                        _stockService.CheckLowStock();
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        private void StockIn()
        {
            DisplayProducts();
            DisplayWarehouses();

            Console.Write("Product ID: ");
            int productId = int.Parse(Console.ReadLine()!);

            Console.Write("Warehouse ID: ");
            int warehouseId = int.Parse(Console.ReadLine()!);

            Console.Write("Quantity: ");
            int quantity = int.Parse(Console.ReadLine()!);

            Console.Write("Reference: ");
            string reference = Console.ReadLine()!;

            _stockService.StockIn(productId, warehouseId, quantity, reference);
        }

        private void StockOut()
        {
            DisplayProducts();
            DisplayWarehouses();

            Console.Write("Product ID: ");
            int productId = int.Parse(Console.ReadLine()!);

            Console.Write("Warehouse ID: ");
            int warehouseId = int.Parse(Console.ReadLine()!);

            Console.Write("Quantity: ");
            int quantity = int.Parse(Console.ReadLine()!);

            Console.Write("Reference: ");
            string reference = Console.ReadLine()!;

            _stockService.StockOut(productId, warehouseId, quantity, reference);
        }

        private void TransferStock()
        {
            DisplayProducts();
            DisplayWarehouses();

            Console.Write("Product ID: ");
            int productId = int.Parse(Console.ReadLine()!);

            Console.Write("From Warehouse ID: ");
            int fromWarehouse = int.Parse(Console.ReadLine()!);

            Console.Write("To Warehouse ID: ");
            int toWarehouse = int.Parse(Console.ReadLine()!);

            Console.Write("Quantity: ");
            int quantity = int.Parse(Console.ReadLine()!);

            _stockService.TransferStock(productId, fromWarehouse, toWarehouse, quantity);
        }

        private void AdjustStock()
        {
            DisplayProducts();
            DisplayWarehouses();

            Console.Write("Product ID: ");
            int productId = int.Parse(Console.ReadLine()!);

            Console.Write("Warehouse ID: ");
            int warehouseId = int.Parse(Console.ReadLine()!);

            Console.Write("New Quantity: ");
            int newQuantity = int.Parse(Console.ReadLine()!);

            Console.Write("Reference: ");
            string reference = Console.ReadLine()!;

            _stockService.AdjustStock(productId, warehouseId, newQuantity, reference);
        }

        private void DisplayProducts()
        {
            Console.WriteLine("\n--- PRODUCTS ---");

            var products = _context.Products.ToList();

            foreach (var p in products)
            {
                Console.WriteLine($"ID: {p.ProductId} | {p.ProductName} | Cost: {p.Cost}");
            }
        }

        private void DisplayWarehouses()
        {
            Console.WriteLine("\n--- WAREHOUSES ---");

            var warehouses = _context.Warehouses.ToList();

            foreach (var w in warehouses)
            {
                Console.WriteLine($"ID: {w.WarehouseId} | {w.WarehouseName}");
            }
        }
    }
}